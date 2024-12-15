using ARMExplorer.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARMExplorer.Controllers
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            TelemetryHelper.LogException(context.Exception);
        }
    }
}