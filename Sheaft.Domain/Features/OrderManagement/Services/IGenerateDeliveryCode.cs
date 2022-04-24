namespace Sheaft.Domain.OrderManagement;

public interface IGenerateDeliveryCode
{
    Result<DeliveryReference> GenerateNextCode(SupplierId supplierIdentifier);
}