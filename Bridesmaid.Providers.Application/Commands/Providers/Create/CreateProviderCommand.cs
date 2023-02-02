using System.Globalization;
using System.Resources;
using Bridesmaid.Providers.Application.Commands.Extensions;
using FluentValidation;
using MediatR;

namespace Bridesmaid.Providers.Application.Commands.Providers.Create;

public class CreateProviderCommand : Command<bool>
{
    public string Name { get; set; }
    public string RegistrationNumber { get; set; }
}

public class CreateProviderCommandValidator : AbstractValidator<CreateProviderCommand>
{
    public CreateProviderCommandValidator(ResourceManager resourceManager, CultureInfo cultureInfo)
    {
        var resourceSet = resourceManager.GetResourceSet(cultureInfo, true, true);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Provider name is required.");
        
        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .WithMessage("Registration number is required.");
    }
}