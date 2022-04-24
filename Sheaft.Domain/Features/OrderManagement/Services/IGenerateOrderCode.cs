namespace Sheaft.Domain.OrderManagement;

public interface IGenerateOrderCode
{
    Result<OrderReference> GenerateNextCode(SupplierId supplierIdentifier);
}