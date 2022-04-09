namespace Sheaft.Domain.OrderManagement;

public record DeliveryProductBatches(ProductId ProductIdentifier, Quantity Quantity, IEnumerable<BatchId> BatchIdentifiers);