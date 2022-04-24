namespace Sheaft.Domain.ProductManagement;

public interface IGenerateProductCode
{
    Result<ProductReference> GenerateNextCode(SupplierId supplierIdentifier);
}