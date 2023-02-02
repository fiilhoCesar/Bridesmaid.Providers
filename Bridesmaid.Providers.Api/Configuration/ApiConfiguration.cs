using System.Globalization;
using Bridesmaid.Providers.Business.Filters;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Bridesmaid.Providers.Api.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var supportedCultures = new[] { new CultureInfo("pt-BR"), new CultureInfo("en-US") };
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        services.AddLocalization();
        services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter))).AddNewtonsoftJson
        (
            x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }
        ).AddDapr();

        services.AddMemoryCache();

        services.AddAutoMapper(typeof(Program));

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
    }

    public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        // app.UseCloudEvents();

        app.UseCors("Total");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health/startup");
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = _ => false });
            endpoints.MapSubscribeHandler();
            endpoints.MapControllers();
        });
    }
}