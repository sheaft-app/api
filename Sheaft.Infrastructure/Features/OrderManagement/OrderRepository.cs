using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

internal class OrderRepository : Repository<Order, OrderId>, IOrderRepository
{
    public OrderRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Order>> Get(OrderId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Order>(ErrorKind.NotFound, "order.not.found");
        });
    }

    public Task<Result<IEnumerable<Order>>> Get(IEnumerable<OrderId> identifiers, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Where(e => identifiers.Contains(e.Identifier))
                .ToListAsync(token);

            return Result.Success(result.AsEnumerable());
        });
    }

    public Task<Result<IEnumerable<Order>>> Get(DeliveryId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Where(e => e.DeliveryIdentifier == identifier)
                .ToListAsync(token);

            return Result.Success(result.AsEnumerable());
        });
    }

    public Task<Result<IEnumerable<Order>>> Get(InvoiceId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Where(e => e.InvoiceIdentifier == identifier)
                .ToListAsync(token);

            return Result.Success(result.AsEnumerable());
        });
    }

    public Task<Result<Maybe<Order>>> FindDraft(CustomerId customerIdentifier, SupplierId supplierIdentifier,
        CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            return Result.Success(await Values
                .SingleOrDefaultAsync(
                    e => e.CustomerIdentifier == customerIdentifier && e.SupplierIdentifier == supplierIdentifier,
                    token) ?? Maybe<Order>.None);
        });
    }
}