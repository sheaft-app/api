namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveProductsToAdjust
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(SupplierId supplierIdentifier, IEnumerable<ProductAdjustment> productAdjustments, CancellationToken token);
}