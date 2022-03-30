namespace Sheaft.Domain.ProductManagement;

public interface IHandleProductCode
{
    Task<Result<ProductCode>> ValidateOrGenerateNextCodeForProduct(string? code, ProductId productIdentifier, 
        SupplierId supplierIdentifier,
        CancellationToken token);
    
    Task<Result<ProductCode>> ValidateOrGenerateNextCodeForProduct(string? code, 
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

    public async Task<Result<ProductCode>> ValidateOrGenerateNextCodeForProduct(string? code, ProductId productIdentifier, 
        SupplierId supplierIdentifier, CancellationToken token)
    {
        return await GenerateNextProductCode(code, productIdentifier, supplierIdentifier, token);
    }

    public async Task<Result<ProductCode>> ValidateOrGenerateNextCodeForProduct(string? code, 
        SupplierId supplierIdentifier, CancellationToken token)
    {
        return await GenerateNextProductCode(code, null, supplierIdentifier, token);
    }

    private async Task<Result<ProductCode>> GenerateNextProductCode(string? code, ProductId? productIdentifier, SupplierId supplierIdentifier, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(code))
            return await _generateProductCode.GenerateNextProductCode(supplierIdentifier, token);

        var existingProductResult = await _productRepository.FindWithCode(new ProductCode(code), supplierIdentifier, token);
        if (existingProductResult.IsFailure)
            return Result.Failure<ProductCode>(existingProductResult);

        if (existingProductResult.Value.HasNoValue)
            return Result.Success(new ProductCode(code));

        if(productIdentifier != null && existingProductResult.Value.Value.Identifier == productIdentifier)
            return Result.Success(new ProductCode(code));

        return Result.Failure<ProductCode>(ErrorKind.Conflict, "product.code.already.exists");
    }
}