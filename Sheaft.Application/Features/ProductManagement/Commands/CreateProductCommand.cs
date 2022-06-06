using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Application.ProductManagement;

public record CreateProductCommand(string Name, string? Code, string? Description, decimal Price, decimal Vat, ReturnableId? ReturnableId, SupplierId SupplierId) : ICommand<Result<string>>;

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
        var codeResult = await _handleProductCode.ValidateOrGenerateNextCode(request.Code, request.SupplierId, token);
        if (codeResult.IsFailure)
            return Result.Failure<string>(codeResult);

        var returnableResult = await _uow.Returnables.Find(request.ReturnableId, token);
        if (returnableResult.IsFailure)
            return Result.Failure<string>(returnableResult);
        
        if(returnableResult.Value.HasNoValue && request.ReturnableId != null)
            return Result.Failure<string>(ErrorKind.NotFound, ReturnableCodes.NotFound);
        
        var product = new Product(new ProductName(request.Name), codeResult.Value, new VatRate(request.Vat), 
            request.Description, request.SupplierId, returnableResult.Value);
        _uow.Products.Add(product);

        var catalogResult = await _retrieveDefaultCatalog.GetOrCreate(request.SupplierId, token);
        if (catalogResult.IsFailure)
            return Result.Failure<string>(catalogResult);

        catalogResult.Value.AddOrUpdateProductPrice(product, new ProductUnitPrice(request.Price));
        _uow.Catalogs.Update(catalogResult.Value);
        
        var result = await _uow.Save(token);
        
        return result.IsSuccess 
            ? Result.Success(product.Id.Value) 
            : Result.Failure<string>(result);
    }
}