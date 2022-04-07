namespace Sheaft.Domain.OrderManagement;

public interface IGenerateDeliveryCode
{
    Task<Result<DeliveryReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}