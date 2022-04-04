namespace Sheaft.Domain.OrderManagement;

public interface IDeliveryRepository : IRepository<Delivery, DeliveryId>
{
    Task<Result<Delivery>> GetDeliveryForOrder(OrderId orderIdentifier, CancellationToken token);
}