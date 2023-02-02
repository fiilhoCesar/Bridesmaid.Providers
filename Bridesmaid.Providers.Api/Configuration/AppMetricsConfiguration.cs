using System.Diagnostics.Metrics;
using Prometheus;
using Serilog;

namespace Bridesmaid.Providers.Api.Configuration;

public static class AppMetricsConfiguration
{
    public static 
#nullable disable
        IApplicationBuilder UseAppMetrics(this IApplicationBuilder app, string name, string help)
    {
        var name1 = name;
        var help1 = help;
        CounterConfiguration counterConfiguration = new CounterConfiguration();
        counterConfiguration.LabelNames = new string[2]
        {
            "method",
            "endpoint"
        };
        var counter = Metrics.CreateCounter(name1, help1, counterConfiguration);
        app.UseSerilogRequestLogging();
        app.Use((Func<HttpContext, Func<Task>, Task>) ((httpContext, next) =>
        {
            counter.WithLabels(new string[2]
            {
                httpContext.Request.Method,
                (string) httpContext.Request.Path
            }).Inc(1.0);
            return next();
        }));
        app.UseMetricServer();
        app.UseHttpMetrics();
        return app;
    }
}