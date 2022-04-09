namespace Sheaft.Domain.OrderManagement;

public interface ICreateDeliveryProductAdjustments
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(Delivery delivery, IEnumerable<ProductAdjustment> productAdjustments, CancellationToken token);
}