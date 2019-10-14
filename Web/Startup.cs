using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Havit.NewProjectTemplate.Web.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Hosting;

namespace Havit.NewProjectTemplate.Web
{
    public class Startup
    {
	    private readonly IConfiguration configuration;

	    public Startup(IConfiguration configuration)
	    {
		    this.configuration = configuration;
	    }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();
			services.AddControllersWithViews();

			services.Configure<WebApiSettings>(configuration.GetSection("AppSettings:WebApi"));

			services.AddExceptionMonitoring(configuration);
	        services.AddApplicationInsightsTelemetry(configuration);

			// In production, the React files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "wwwroot/dist";
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
                app.UseStatusCodePages();
			}

			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseExceptionMonitoring();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Default}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				//if (env.IsDevelopment())
				//{
				//	spa.UseReactDevelopmentServer(npmScript: "start");
				//}
			});
		}
    }
}
