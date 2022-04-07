namespace Sheaft.Domain.OrderManagement;

public record ProductBatches(ProductId ProductIdentifier, IEnumerable<BatchId> BatchIdentifiers);