namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveProductsToAdjust
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(Delivery delivery, IEnumerable<ProductAdjustment> productAdjustments, CancellationToken token);
}
public interface IRetrieveDeliveryBatches
{
    Task<Result<IEnumerable<DeliveryBatch>>> Get(Delivery delivery, IEnumerable<ProductBatches> productBatches, CancellationToken token);
}