using ARMExplorer.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ARMExplorer.Controllers
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            TelemetryHelper.LogException(context.Exception);
        }
    }
}