using ARMExplorer.App_Start;
using System.Configuration;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace ARMExplorer
{
    public class Global : System.Web.HttpApplication
    {

        protected void Startup(IConfiguration configuration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration
                .Active.InstrumentationKey = System.Environment.GetEnvironmentVariable("AppInsightKey") ?? string.Empty;

        }
    }
}
