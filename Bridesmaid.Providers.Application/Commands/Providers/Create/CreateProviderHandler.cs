using Bridesmaid.Providers.Application.Commands.Extensions;
using MediatR;

namespace Bridesmaid.Providers.Application.Commands.Providers.Create;

public class CreateProviderHandler : CommandHandler, IRequestHandler<CreateProviderCommand, CommandResponse<bool>>
{
    public Task<CommandResponse<bool>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}