// using System.Web.Routing;
using Microsoft.AspNetCore.SystemWebAdapters;
using Microsoft.Extensions.Configuration;

namespace ARMExplorer.App_Start
{
    public class WebApiConfig
    {
        public static void Register(IConfiguration config)
        {
        //     app.MapControllerRoute("get-operations-for-user-post", "api/all-operations", new { controller = "Operation", action = "GetPost" }, new { verb = new HttpMethodConstraint("POST") });
        //     app.MapControllerRoute("get-providers-for-subscription", "api/operations/providers/{subscriptionId}", new { controller = "Operation", action = "GetProviders" }, new { verb = new HttpMethodConstraint("GET", "HEAD") });
        //     app.MapControllerRoute("invoke-operation", "api/operations", new { controller = "Operation", action = "Invoke" }, new { verb = new HttpMethodConstraint("POST") });
        //     app.MapControllerRoute("get-providers-for-user", "api/providers",new {controller = "Operation", action = "GetAllProviders" }, new {verb = new HttpMethodConstraint("GET", "HEAD")});

        //     app.MapControllerRoute("get-token", "api/token", new { controller = "ARM", action = "GetToken" }, new { verb = new HttpMethodConstraint("GET", "HEAD") });
        //     app.MapControllerRoute("get-search", "api/search", new { controller = "ARM", action = "Search" }, new { verb = new HttpMethodConstraint("GET", "HEAD") });
        //     app.MapControllerRoute("get", "api/{*path}", new { controller = "ARM", action = "Get" }, new { verb = new HttpMethodConstraint("GET", "HEAD") });
        }
    }
}