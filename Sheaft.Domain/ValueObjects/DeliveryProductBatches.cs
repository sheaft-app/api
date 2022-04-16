namespace Sheaft.Domain.OrderManagement;

public record DeliveryProductBatches(DeliveryOrder Order, ProductId ProductIdentifier, Quantity Quantity, IEnumerable<BatchId> BatchIdentifiers);