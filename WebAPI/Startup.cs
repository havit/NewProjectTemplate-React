﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Havit.NewProjectTemplate.WebAPI.Infrastructure;
using Havit.NewProjectTemplate.WebAPI.Infrastructure.ConfigurationExtensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Havit.NewProjectTemplate.WebAPI.Infrastructure.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Havit.AspNetCore.Mvc.ExceptionMonitoring.Filters;
using Havit.NewProjectTemplate.WebAPI.Infrastructure.Security;
using Havit.NewProjectTemplate.Facades.Infrastructure.Security.Authentication;
using Havit.NewProjectTemplate.Facades.Infrastructure.Security.Claims;
using Havit.NewProjectTemplate.DependencyInjection;
using Havit.AspNetCore.Mvc.ExceptionMonitoring.Services;

[assembly: ApiControllerAttribute]

namespace Havit.NewProjectTemplate.WebAPI
{
    public class Startup
    {
	    private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
	        this.configuration = configuration;
        }

        /// <summary>
        /// Configure services.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
			services.ConfigureForWebAPI(configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions(); // Adds services required for using options.

	        services.AddCustomizedRequestLocalization();
			services.AddCustomizedMvc(configuration);
            services.AddAuthorization();
            services.AddCustomizedAuthentication(configuration); // musí být voláno až po AddMvc, jinak nejsou volány IClaimsTransformation.
	        services.AddCustomizedMailing(configuration);
	        
			services.AddExceptionMonitoring(configuration);
			services.AddCustomizedErrorToJson();

            services.AddCustomizedCors(configuration);
            services.AddCustomizedOpenApi();			

	        services.AddApplicationInsightsTelemetry(configuration);

			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			services.AddTransient<ErrorMonitoringFilter>();
			services.AddScoped<IApplicationAuthenticationService, ApplicationAuthenticationService>();
			services.AddScoped<IUserContextInfoBuilder, Infrastructure.Security.UserContextInfoBuilder>();

		}

		/// <summary>
		/// Configure middleware.
		/// </summary>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<Havit.NewProjectTemplate.WebAPI.Infrastructure.Cors.CorsOptions> corsOptions, IExceptionMonitoringService exceptionMonitoringService)
        {
			try
			{
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}

				app.UseStaticFiles();

				app.UseRequestLocalization();

				app.UseExceptionMonitoring();
				app.UseErrorToJson();

				app.UseRouting();

				app.UseCustomizedCors(corsOptions);
				app.UseAuthentication();
				app.UseAuthorization();

				app.UseEndpoints(endpoints => endpoints.MapControllers());

				app.UseCustomizedOpenApiSwaggerUI();

				app.UpgradeDatabaseSchemaAndData();
			}
			catch (Exception exception)
			{
				exceptionMonitoringService.HandleException(exception);
				throw;
			}
        }

    }
}
