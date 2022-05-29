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

        var existingProductResult = await _productRepository.Find(new ProductReference(code), supplierIdentifier, token);
        if (existingProductResult.IsFailure)
            return Result.Failure<ProductReference>(existingProductResult);

        if (existingProductResult.Value.HasNoValue)
            return Result.Success(new ProductReference(code));

        if(productIdentifier != null && existingProductResult.Value.Value.Id == productIdentifier)
            return Result.Success(new ProductReference(code));

        return Result.Failure<ProductReference>(ErrorKind.Conflict, "product.code.already.exists");
    }
}