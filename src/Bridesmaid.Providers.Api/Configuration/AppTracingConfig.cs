using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;

namespace Bridesmaid.Providers.Api.Configuration;

public static class AppTracingConfig
{
    public static
#nullable disable
        IServiceCollection AddAppTelemetryTracing(this IServiceCollection services)
    {
        services.AddOpenTracing();
        if (Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST") == null)
            return services;
        services.AddSingleton<ITracer>((Func<IServiceProvider, ITracer>)(serviceProvider =>
        {
            var requiredService = serviceProvider.GetRequiredService<ILoggerFactory>();
            var senderConfiguration =
                new Jaeger.Configuration.SenderConfiguration(requiredService)
                    .WithAgentHost(Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST"))
                    .WithAgentPort(new int?(Convert.ToInt32(Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT"))));
            Jaeger.Configuration.SenderConfiguration.DefaultSenderResolver =
                new SenderResolver(requiredService).RegisterSenderFactory<ThriftSenderFactory>();
            var configuration = Jaeger.Configuration.FromEnv(requiredService);
            var samplerConfig =
                new Jaeger.Configuration.SamplerConfiguration(requiredService).WithType("const")
                    .WithParam(1.0);
            var reporterConfig =
                new Jaeger.Configuration.ReporterConfiguration(requiredService).WithSender(senderConfiguration)
                    .WithLogSpans(true);
            ITracer tracer = configuration.WithSampler(samplerConfig).WithReporter(reporterConfig).GetTracer();
            GlobalTracer.Register(tracer);
            return tracer;
        }));
        services.Configure<AspNetCoreDiagnosticOptions>(options =>
        {
            options.Hosting.IgnorePatterns.Add((Func<HttpContext, bool>)(context =>
            {
                var str = context.Request.Path.Value;
                return str != null && str.StartsWith("/status");
            }));
            options.Hosting.IgnorePatterns.Add((Func<HttpContext, bool>)(context =>
            {
                var str = context.Request.Path.Value;
                return str != null && str.StartsWith("/metrics");
            }));
            options.Hosting.IgnorePatterns.Add((Func<HttpContext, bool>)(context =>
            {
                var str = context.Request.Path.Value;
                return str != null && str.StartsWith("/swagger");
            }));
            options.Hosting.IgnorePatterns.Add((Func<HttpContext, bool>)(context =>
            {
                var str = context.Request.Path.Value;
                return str != null && str.StartsWith("/stats");
            }));
        });
        return services;
    }
}