using System.Diagnostics;
using System.Net;
using System.Runtime.Loader;
using Bridesmaid.Providers.Business.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenTracing;
using Serilog;

namespace Bridesmaid.Providers.Business.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ITracer _tracer;

    public ExceptionFilter(ITracer tracer) => _tracer = tracer;

    public override void OnException(ExceptionContext context)
    {
        var httpStatusCode = context.HttpContext.Response.StatusCode;
        context.HttpContext.Response.ContentType = "application/json";
        var baseRequestResult = new
        {
            context.Exception.Message,
            context.Exception.StackTrace
        };
        Log.Error(context.Exception, context.Exception.Message);
        _tracer.Log(baseRequestResult);
        var objectResult = new ObjectResult((object) baseRequestResult)
        {
            StatusCode = httpStatusCode
        };
        context.Result = objectResult;
    }
}