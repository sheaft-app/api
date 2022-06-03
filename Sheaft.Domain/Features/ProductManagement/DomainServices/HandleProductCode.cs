namespace Sheaft.Domain.ProductManagement;

public interface IHandleProductCode
{
    Task<Result<ProductReference>> ValidateOrGenerateNextCodeForProduct(string? code, ProductId productIdentifier, 
        SupplierId supplierIdentifier,
        CancellationToken token);
    
    Task<Result<ProductReference>> ValidateOrGenerateNextCode(string? code, 
        SupplierId supplierIdentifier,
        CancellationToken token);
}

internal class HandleProductCode : IHandleProductCode
{
    private readonly IProductRepository _productRepository;
    private readonly IGenerateProductCode _generateProductCode;

    public HandleProductCode(
        IProductRepository productRepository,
        IGenerateProductCode generateProductCode)
    {
        _productRepository = productRepository;
        _generateProductCode = generateProductCode;
    }

    public async Task<Result<ProductReference>> ValidateOrGenerateNextCodeForProduct(string? code, ProductId productIdentifier, 
        SupplierId supplierIdentifier, CancellationToken token)
    {
        return await GenerateNextProductCode(code, productIdentifier, supplierIdentifier, token);
    }

    public async Task<Result<ProductReference>> ValidateOrGenerateNextCode(string? code, 
        SupplierId supplierIdentifier, CancellationToken token)
    {
        return await GenerateNextProductCode(code, null, supplierIdentifier, token);
    }

    private async Task<Result<ProductReference>> GenerateNextProductCode(string? code, ProductId? productIdentifier, SupplierId supplierIdentifier, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(code))
            return _generateProductCode.GenerateNextCode(supplierIdentifier);

        var productReference = new ProductReference(code.Replace(" ", "-").ToUpperInvariant());
        var existingProductResult = await _productRepository.Find(productReference, supplierIdentifier, token);
        if (existingProductResult.IsFailure)
            return Result.Failure<ProductReference>(existingProductResult);

        if (existingProductResult.Value.HasNoValue)
            return Result.Success(productReference);

        if(productIdentifier != null && existingProductResult.Value.Value.Id == productIdentifier)
            return Result.Success(productReference);

        return Result.Failure<ProductReference>(ErrorKind.Conflict, "product.code.already.exists");
    }
}