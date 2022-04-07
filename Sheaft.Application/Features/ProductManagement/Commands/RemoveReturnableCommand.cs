using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record RemoveReturnableCommand(ReturnableId Identifier) : ICommand<Result>;

internal class RemoveReturnableHandler : ICommandHandler<RemoveReturnableCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public RemoveReturnableHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task<Result> Handle(RemoveReturnableCommand request, CancellationToken token)
    {
        var returnableResult = await _uow.Returnables.Get(request.Identifier, token);
        if (returnableResult.IsFailure)
            return returnableResult;
        
        _uow.Returnables.Remove(returnableResult.Value);
        return await _uow.Save(token);
    }
}