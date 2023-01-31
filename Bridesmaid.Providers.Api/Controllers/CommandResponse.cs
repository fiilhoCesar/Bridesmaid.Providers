using FluentValidation.Results;

namespace Bridesmaid.Providers.Api.Controllers;

public class CommandResponse<TResponse>
{
    public ValidationResult ValidationResult { get; set; }

    public TResponse Response { get; set; }
}