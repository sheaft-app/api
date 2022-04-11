using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record UpdateProductCommand(ProductId Identifier, string Name, int Vat, string? Code, string? Description, int Price, ReturnableId? ReturnableIdentifier) : ICommand<Result>;

internal class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveDefaultCatalog _retrieveDefaultCatalog;
    private readonly IHandleProductCode _handleProductCode;
    private readonly IReturnableRepository _returnableRepository;

    public UpdateProductHandler(
        IUnitOfWork uow,
        IRetrieveDefaultCatalog retrieveDefaultCatalog,
        IHandleProductCode handleProductCode)
    {
        _uow = uow;
        _retrieveDefaultCatalog = retrieveDefaultCatalog;
        _handleProductCode = handleProductCode;
    }
    
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken token)
    {
        var productResult = await _uow.Products.Get(request.Identifier, token);
        if (productResult.IsFailure)
            return Result.Failure(productResult);

        var product = productResult.Value;
        var catalogResult = await _retrieveDefaultCatalog.GetOrCreate(product.SupplierIdentifier, token);
        if (catalogResult.IsFailure)
            return Result.Failure(catalogResult);

        var updateResult = product.UpdateInfo(new ProductName(request.Name), new VatRate(request.Vat), request.Description);
        if (updateResult.IsFailure)
            return updateResult;
        
        if (request.Code != product.Reference.Value)
        {
            var codeResult = await _handleProductCode.ValidateOrGenerateNextCodeForProduct(request.Code, product.Identifier, product.SupplierIdentifier, token);
            if (codeResult.IsFailure)
                return Result.Failure(codeResult);
            
            var updateCodeResult = product.UpdateCode(codeResult.Value);
            if (updateCodeResult.IsFailure)
                return updateCodeResult;
        }

        Returnable? returnable = null;
        if (request.ReturnableIdentifier != null)
        {
            var returnableResult = await _uow.Returnables.Get(request.ReturnableIdentifier, token);
            if (returnableResult.IsFailure)
                return Result.Failure(returnableResult);

            returnable = returnableResult.Value;
        }
        
        var assignResult = product.SetReturnable(returnable);
        if (assignResult.IsFailure)
            return assignResult;
        
        _uow.Products.Update(product);
        
        var catalog = catalogResult.Value;
        var catalogPriceResult = catalog.AddOrUpdateProductPrice(product, new ProductUnitPrice(request.Price));
        if (catalogPriceResult.IsFailure)
            return catalogPriceResult;
        
        _uow.Catalogs.Update(catalog);
        return await _uow.Save(token);
    }
}