namespace Sheaft.Domain.ProductManagement;

public interface IGenerateProductCode
{
    Task<Result<ProductCode>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}