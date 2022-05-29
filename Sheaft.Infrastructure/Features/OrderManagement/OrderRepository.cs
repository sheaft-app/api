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
                .SingleOrDefaultAsync(e => e.Id == identifier, token);

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
                .Where(e => identifiers.Contains(e.Id))
                .ToListAsync(token);

            if (result.Count() != identifiers.Count())
                return Result.Failure<IEnumerable<Order>>(ErrorKind.NotFound, "orders.some.not.found");

            return Result.Success(result.AsEnumerable());
        });
    }

    public Task<Result<IEnumerable<Order>>> Find(DeliveryId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Where(e => e.DeliveryId == identifier)
                .ToListAsync(token);
            
            return Result.Success(result.AsEnumerable());
        });
    }

    public Task<Result<IEnumerable<Order>>> Find(InvoiceId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Where(e => e.InvoiceId == identifier)
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
                    e => e.CustomerId == customerIdentifier && e.SupplierId == supplierIdentifier,
                    token) ?? Maybe<Order>.None);
        });
    }
}