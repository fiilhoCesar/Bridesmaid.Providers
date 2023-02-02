using Bridesmaid.Providers.Business.Repositories;
using FluentValidation.Results;
using MediatR;

namespace Bridesmaid.Providers.Application.Commands.Extensions;

public abstract class CommandHandler
{
    protected
#nullable disable
        ValidationResult ValidationResult;

    protected CommandHandler() => this.ValidationResult = new ValidationResult();

    protected void AddError(string message) =>
        this.ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));

    protected void AddValidationResult(ValidationResult validationResult) => this.ValidationResult = validationResult;

    protected bool ValidOperation() => !this.ValidationResult.Errors.Any<ValidationFailure>();

    protected CommandResponse<TResponse> ReturnReply<TResponse>(TResponse response)
    {
        if (!this.ValidOperation())
            return new CommandResponse<TResponse>()
            {
                ValidationResult = this.ValidationResult
            };
        return new CommandResponse<TResponse>()
        {
            Response = response
        };
    }

    protected async Task SaveData(IUnitOfWork uow)
    {
        if (await uow.Save())
            return;
        this.AddError("Houve um erro ao salvar os dados");
    }
}

public abstract class Command<TResponse> :
    Message,
    IRequest<CommandResponse<TResponse>>,
    IBaseRequest
{
    public DateTime Timestamp { get; private set; }

    public CommandResponse<TResponse> CommandResponse { get; set; }

    protected Command() => this.Timestamp = DateTime.Now;
}

public class CommandResponse<TResponse>
{
    public ValidationResult ValidationResult { get; set; }

    public TResponse Response { get; set; }
}

public abstract class Message
{
    public string MessageType { get; protected set; }

    public Guid AggregateId { get; protected set; }

    protected Message() => this.MessageType = this.GetType().Name;
}