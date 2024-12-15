using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ARMExplorer.App_Start
{
    public class WebApiConfig
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void RegisterRoutes(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "get-operations-for-user-post",
                pattern: "api/all-operations",
                defaults: new { controller = "Operation", action = "GetPost" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "POST" }));

            endpoints.MapControllerRoute(
                name: "get-providers-for-subscription",
                pattern: "api/operations/providers/{subscriptionId}",
                defaults: new { controller = "Operation", action = "GetProviders" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "GET", "HEAD" }));

            endpoints.MapControllerRoute(
                name: "invoke-operation",
                pattern: "api/operations",
                defaults: new { controller = "Operation", action = "Invoke" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "POST" }));

            endpoints.MapControllerRoute(
                name: "get-providers-for-user",
                pattern: "api/providers",
                defaults: new { controller = "Operation", action = "GetAllProviders" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "GET", "HEAD" }));

            endpoints.MapControllerRoute(
                name: "get-token",
                pattern: "api/token",
                defaults: new { controller = "ARM", action = "GetToken" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "GET", "HEAD" }));

            endpoints.MapControllerRoute(
                name: "get-search",
                pattern: "api/search",
                defaults: new { controller = "ARM", action = "Search" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "GET", "HEAD" }));

            endpoints.MapControllerRoute(
                name: "get",
                pattern: "api/{*path}",
                defaults: new { controller = "ARM", action = "Get" }
            ).WithMetadata(new HttpMethodMetadata(new[] { "GET", "HEAD" }));
        }
    }
}