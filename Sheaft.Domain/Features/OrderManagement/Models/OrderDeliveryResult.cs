namespace Sheaft.Domain.OrderManagement;

public record OrderDeliveryResult(Order Order, Delivery? Delivery);
public record DeliveryOrdersResult(Delivery Delivery, IEnumerable<Order> Orders);