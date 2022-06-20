using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record RemoveReturnableCommand(ReturnableId Identifier, SupplierId SupplierId) : ICommand<Result>;

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

        var productsResult = await _uow.Products.WithReturnable(request.Identifier, token);
        if (productsResult.IsFailure)
            return productsResult;
        
        foreach (var product in productsResult.Value)
        {
            product.SetReturnable(Maybe<Returnable>.None);
            _uow.Products.Update(product);
        }

        _uow.Returnables.Remove(returnableResult.Value);
        return await _uow.Save(token);
    }
}