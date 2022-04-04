namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveOrderCustomer
{
    Task<Result<DeliveryAddress>> GetDeliveryAddress(OrderId orderIdentifier, CancellationToken token);
}