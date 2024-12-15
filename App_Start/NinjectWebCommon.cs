using System;
using ARMExplorer.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace ARMExplorer.App_Start
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add application services here
            services.AddControllersWithViews();
            WebApiConfig.RegisterServices(services);
            // Configure authentication
            services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
                .AddBearerToken();
            // Add other services here
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (lifetime == null)
            {
                throw new ArgumentNullException(nameof(lifetime));
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                WebApiConfig.RegisterRoutes(endpoints);
            });

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);
        }

        private void OnStarted()
        {
            // Code to run when the application starts
        }

        private void OnStopping()
        {
            // Code to run when the application stops
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<ARMExplorer.App_Start.Startup>();
                });
    }
}