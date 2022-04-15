namespace Sheaft.Domain.OrderManagement;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<Result<IEnumerable<Order>>> Get(IEnumerable<OrderId> identifiers, CancellationToken token);
    Task<Result<Maybe<Order>>> FindDraft(CustomerId customerIdentifier, SupplierId supplierIdentifier,
        CancellationToken token);
}