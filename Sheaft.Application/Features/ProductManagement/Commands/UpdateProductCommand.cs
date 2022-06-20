using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record UpdateProductCommand(SupplierId SupplierId, ProductId Identifier, string Name, decimal Vat, string? Code,
    string? Description, decimal Price, ReturnableId? ReturnableId) : ICommand<Result>;

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

        var codeResult = await _handleProductCode.ValidateOrGenerateNextCodeForProduct(request.Code, product.Id, product.SupplierId, token);
        if (codeResult.IsFailure)
            return Result.Failure(codeResult);
        
        product.UpdateInfo(codeResult.Value, new ProductName(request.Name), new VatRate(request.Vat), request.Description);

        var returnableResult = await _uow.Returnables.Find(request.ReturnableId, token);
        if (returnableResult.IsFailure)
            return Result.Failure<string>(returnableResult);
        
        if(returnableResult.Value.HasNoValue && request.ReturnableId != null)
            return Result.Failure<string>(ErrorKind.NotFound, "returnable.not.found");
        
        product.SetReturnable(returnableResult.Value);
        _uow.Products.Update(product);
        
        var catalogResult = await _retrieveDefaultCatalog.GetOrCreate(product.SupplierId, token);
        if (catalogResult.IsFailure)
            return Result.Failure(catalogResult);
        
        catalogResult.Value.AddOrUpdateProductPrice(product, new ProductUnitPrice(request.Price));
        
        _uow.Catalogs.Update(catalogResult.Value);
        return await _uow.Save(token);
    }
}