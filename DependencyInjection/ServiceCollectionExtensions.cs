﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using Havit.NewProjectTemplate.DataLayer.DataSources.Security;
using Havit.NewProjectTemplate.DataLayer.Seeds.Core.Common;
using Havit.NewProjectTemplate.Entity;
using Havit.NewProjectTemplate.Services.Infrastructure;
using Havit.NewProjectTemplate.Services.Infrastructure.TimeService;
using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Extensions.DependencyInjection;
using Havit.Services;
using Havit.Services.TimeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Havit.Services.Caching;
using System.Runtime.Caching;
using Microsoft.Extensions.DependencyInjection;
using Havit.Data.EntityFrameworkCore.Patterns.DependencyInjection;

namespace Havit.NewProjectTemplate.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IServiceCollection ConfigureForWebAPI(this IServiceCollection services, IConfiguration configuration)
		{
			InstallConfiguration installConfiguration = new InstallConfiguration
			{
				DatabaseConnectionString = configuration.GetConnectionString("Database"),
				ServiceProfiles = new[] { ServiceAttribute.DefaultProfile, ServiceProfiles.WebAPI },
			};

			return services.ConfigureForAll(installConfiguration);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IServiceCollection ConfigureForTests(this IServiceCollection services, bool useInMemoryDb = true)
		{
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			if (string.IsNullOrEmpty(environment))
			{
				environment = "Development";
			}

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{environment}.json", true)
				.Build();

			InstallConfiguration installConfiguration = new InstallConfiguration
			{
				DatabaseConnectionString = configuration.GetConnectionString("Database"),
				ServiceProfiles = new[] { ServiceAttribute.DefaultProfile },
				UseInMemoryDb = useInMemoryDb,
			};

			return services.ConfigureForAll(installConfiguration);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static IServiceCollection ConfigureForAll(this IServiceCollection services, InstallConfiguration installConfiguration)
		{
			InstallHavitEntityFramework(services, installConfiguration);
			InstallHavitServices(services);
			InstallByServiceAttribute(services, installConfiguration);
			InstallAuthorizationHandlers(services);

			services.AddMemoryCache(); // ie. IClaimsCacheStorage

			return services;
		}

		private static void InstallHavitEntityFramework(IServiceCollection services, InstallConfiguration configuration)
		{
			DbContextOptions options = configuration.UseInMemoryDb
				? new DbContextOptionsBuilder<NewProjectTemplateDbContext>().UseInMemoryDatabase(nameof(NewProjectTemplateDbContext)).Options
				: new DbContextOptionsBuilder<NewProjectTemplateDbContext>().UseSqlServer(configuration.DatabaseConnectionString, c => c.MaxBatchSize(30)).Options;

			services.WithEntityPatternsInstaller()
				.AddEntityPatterns()
				//.AddLocalizationServices<Language>()
				.AddDbContext<NewProjectTemplateDbContext>(options)
				.AddDataLayer(typeof(ILoginAccountDataSource).Assembly);
		}

		private static void InstallHavitServices(IServiceCollection services)
		{
			// HAVIT .NET Framework Extensions
			services.AddSingleton<ITimeService, ApplicationTimeService>();
			services.AddSingleton<ICacheService, MemoryCacheService>();
			services.AddSingleton(new MemoryCacheServiceOptions { UseCacheDependenciesSupport = false });
		}

		private static void InstallByServiceAttribute(IServiceCollection services, InstallConfiguration configuration)
		{

			services.AddByServiceAttribute(typeof(Havit.NewProjectTemplate.DataLayer.Properties.AssemblyInfo).Assembly, configuration.ServiceProfiles);
			services.AddByServiceAttribute(typeof(Havit.NewProjectTemplate.Services.Properties.AssemblyInfo).Assembly, configuration.ServiceProfiles);
			services.AddByServiceAttribute(typeof(Havit.NewProjectTemplate.Facades.Properties.AssemblyInfo).Assembly, configuration.ServiceProfiles);
		}

		private static void InstallAuthorizationHandlers(IServiceCollection services)
		{
			services.Scan(scan => scan.FromAssemblyOf<Havit.NewProjectTemplate.Services.Properties.AssemblyInfo>()
				.AddClasses(classes => classes.AssignableTo<IAuthorizationHandler>())
				.As<IAuthorizationHandler>()
				.WithScopedLifetime()
			);
		}
	}
}