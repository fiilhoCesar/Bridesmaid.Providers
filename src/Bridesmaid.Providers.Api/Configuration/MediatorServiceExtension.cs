using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;

namespace Bridesmaid.Providers.Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class MediatorServiceExtension
    {
        public static void AddMediator(this IServiceCollection services)
        {
            const string applicationAssemblyName = "User.Onboard.Application";
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
            services.AddMediatR(AppDomain.CurrentDomain.Load(applicationAssemblyName));
        }
    }
}