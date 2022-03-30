namespace Sheaft.Domain.ProductManagement;

public interface IGenerateProductCode
{
    Task<Result<ProductCode>> GenerateNextProductCode(SupplierId supplierIdentifier, CancellationToken token);
}