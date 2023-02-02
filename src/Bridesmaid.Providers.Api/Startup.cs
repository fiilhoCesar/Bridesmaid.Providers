using System.Text.Json;
using Bridesmaid.Providers.Api.Configuration;
using DateOnlyTimeOnly.AspNet.Converters;

namespace Bridesmaid.Providers.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiConfiguration(Configuration);
        services.AddSwaggerConfiguration();
        services.AddMediator();

        #region DependencyInjection

        services.AddResourceManager();
        // services.AddApplicationServices();
        // services.AddRepositoryServices();

        #endregion

        services.AddAppTelemetryTracing();
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new DateOnlyJsonConverter());
        jsonOptions.Converters.Add(new TimeOnlyJsonConverter());
        services.AddDaprClient(options => options.UseJsonSerializationOptions(jsonOptions));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestLocalization();
        app.UseAppMetrics("providersApi", "Manages requests to Providers API.");
        app.UseApiConfiguration(env);
        app.UseSwaggerConfiguration();
    }
}