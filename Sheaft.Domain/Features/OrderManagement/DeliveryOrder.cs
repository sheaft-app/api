namespace Sheaft.Domain.OrderManagement;

public record DeliveryOrder(OrderReference Reference, DateTimeOffset PublishedOn);