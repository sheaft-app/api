namespace Sheaft.Domain.OrderManagement;

public interface IGenerateDeliveryCode
{
    Task<Result<DeliveryCode>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}