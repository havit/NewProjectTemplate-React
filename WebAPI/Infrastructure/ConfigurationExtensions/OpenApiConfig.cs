using System;
using System.IO;
using Havit.NewProjectTemplate.Facades.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.NewProjectTemplate.WebAPI.Infrastructure.ConfigurationExtensions
{
    public static class OpenApiConfig
	{
        public static void AddCustomizedOpenApi(this IServiceCollection services)
        {
			services.AddOpenApiDocument(c =>
			{
				c.DocumentName = "current";
				c.Title = "NewProjectTemplate";
				c.Version = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(AssemblyInfo).Assembly.Location).ProductVersion;
			});
        }

        public static void UseCustomizedOpenApiSwaggerUI(this IApplicationBuilder app)
        {
			app.UseOpenApi();
			app.UseSwaggerUi3();
		}
    }
}
