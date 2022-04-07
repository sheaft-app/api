namespace Sheaft.Domain.ProductManagement;

public interface IGenerateProductCode
{
    Task<Result<ProductReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}