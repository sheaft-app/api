namespace Sheaft.Domain.OrderManagement;

public interface ICreateDeliveryBatches
{
    Task<Result<IEnumerable<DeliveryBatch>>> Get(Delivery delivery, IEnumerable<DeliveryProductBatches> productBatches, CancellationToken token);
}