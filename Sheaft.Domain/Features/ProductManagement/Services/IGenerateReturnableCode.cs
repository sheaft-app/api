namespace Sheaft.Domain.ProductManagement;

public interface IGenerateReturnableCode
{
    Result<ReturnableReference> GenerateNextCode(SupplierId supplierIdentifier);
}