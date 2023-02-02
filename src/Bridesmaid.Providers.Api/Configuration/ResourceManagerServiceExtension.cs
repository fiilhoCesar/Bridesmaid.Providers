using System.Globalization;
using System.Resources;
using Bridesmaid.Providers.Api.Resources;

namespace Bridesmaid.Providers.Api.Configuration;

public static class ResourceManagerServiceExtension
{
    public static void AddResourceManager(this IServiceCollection services)
    {
        services.AddSingleton(new ResourceManager(typeof(ProvidersResources)));
        services.AddScoped(provider =>
        {
            var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;

            if (httpContext == null)
            {
                return CultureInfo.InvariantCulture;
            }

            return httpContext.Request.Headers.TryGetValue("language", out var language)
                ? new CultureInfo(language)
                : CultureInfo.InvariantCulture;
        });
    }
}