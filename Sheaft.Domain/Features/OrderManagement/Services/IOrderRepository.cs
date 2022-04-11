namespace Sheaft.Domain.OrderManagement;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<Result<Maybe<Order>>> FindDraft(CustomerId customerIdentifier, SupplierId supplierIdentifier,
        CancellationToken token);
}