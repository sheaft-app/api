using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record UpdateProductCommand(ProductId Identifier, string Name, int Vat, string? Code, string? Description, int Price) : ICommand<Result>;

internal class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveDefaultCatalog _retrieveDefaultCatalog;
    private readonly IHandleProductCode _handleProductCode;

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

        product.UpdateInfo(new ProductName(request.Name), new VatRate(request.Vat), request.Description);
        if (request.Code != product.Code.Value)
        {
            var codeResult = await _handleProductCode.ValidateOrGenerateNextCodeForProduct(request.Code, product.Identifier, product.SupplierIdentifier, token);
            if (codeResult.IsFailure)
                return Result.Failure(codeResult);
            
            product.UpdateCode(codeResult.Value);
        }
        
        _uow.Products.Update(product);
        
        var catalog = catalogResult.Value;
        catalog.AddOrUpdateProductPrice(product, new ProductPrice(request.Price));
        
        _uow.Catalogs.Update(catalog);
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success() 
            : Result.Failure(result);
    }
}