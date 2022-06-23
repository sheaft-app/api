using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record RemoveProductCommand(ProductId Identifier, SupplierId SupplierId) : Command<Result>;

internal class RemoveProductHandler : ICommandHandler<RemoveProductCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveDefaultCatalog _retrieveDefaultCatalog;

    public RemoveProductHandler(
        IUnitOfWork uow,
        IRetrieveDefaultCatalog retrieveDefaultCatalog)
    {
        _uow = uow;
        _retrieveDefaultCatalog = retrieveDefaultCatalog;
    }

    public async Task<Result> Handle(RemoveProductCommand request, CancellationToken token)
    {
        var productResult = await _uow.Products.Get(request.Identifier, token);
        if (productResult.IsFailure)
            return Result.Failure(productResult);

        var product = productResult.Value;
        var catalogResult = await _retrieveDefaultCatalog.GetOrCreate(product.SupplierId, token);
        if (catalogResult.IsFailure)
            return Result.Failure(catalogResult);

        var catalog = catalogResult.Value;
        var removeResult = catalog.RemoveProduct(product);
        if (removeResult.IsFailure)
            return removeResult;

        _uow.Products.Remove(product);
        _uow.Catalogs.Update(catalog);

        return await _uow.Save(token);
    }
}