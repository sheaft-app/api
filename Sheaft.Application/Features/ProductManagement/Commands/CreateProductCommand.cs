using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record CreateProductCommand(string Name, string? Code, string? Description, int Price, int Vat, ReturnableId? ReturnableIdentifier, SupplierId SupplierIdentifier) : ICommand<Result<string>>;

internal class CreateProductHandler : ICommandHandler<CreateProductCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IRetrieveDefaultCatalog _retrieveDefaultCatalog;
    private readonly IHandleProductCode _handleProductCode;

    public CreateProductHandler(
        IUnitOfWork uow, 
        IRetrieveDefaultCatalog retrieveDefaultCatalog,
        IHandleProductCode handleProductCode)
    {
        _uow = uow;
        _retrieveDefaultCatalog = retrieveDefaultCatalog;
        _handleProductCode = handleProductCode;
    }
    
    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken token)
    {
        var codeResult = await _handleProductCode.ValidateOrGenerateNextCodeForProduct(request.Code, request.SupplierIdentifier, token);
        if (codeResult.IsFailure)
            return Result.Failure<string>(codeResult);
        
        var catalogResult = await _retrieveDefaultCatalog.GetOrCreate(request.SupplierIdentifier, token);
        if (catalogResult.IsFailure)
            return Result.Failure<string>(catalogResult);

        Returnable? returnable = null;
        if (request.ReturnableIdentifier != null)
        {
            var returnableResult = await _uow.Returnables.Get(request.ReturnableIdentifier, token);
            if (returnableResult.IsFailure)
                return Result.Failure<string>(returnableResult);

            returnable = returnableResult.Value;
        }
        
        var product = new Product(new ProductName(request.Name), codeResult.Value, new VatRate(request.Vat), request.Description, request.SupplierIdentifier, returnable);
        _uow.Products.Add(product);

        var catalog = catalogResult.Value;
        catalog.AddOrUpdateProductPrice(product, new ProductUnitPrice(request.Price));
        _uow.Catalogs.Update(catalog);
        
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success(product.Identifier.Value) 
            : Result.Failure<string>(result);
    }
}