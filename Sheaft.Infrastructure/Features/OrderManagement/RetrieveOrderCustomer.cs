using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class RetrieveOrderCustomer : IRetrieveOrderCustomer
{
    private readonly IDbContext _context;

    public RetrieveOrderCustomer(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DeliveryAddress>> GetDeliveryAddress(OrderId orderIdentifier, CancellationToken token)
    {
        try
        {
            var order = await _context.Set<Order>().SingleOrDefaultAsync(o => o.Id == orderIdentifier, token);
            if (order == null)
                return Result.Failure<DeliveryAddress>(ErrorKind.NotFound, "order.not.found");

            var customer = await _context.Set<Customer>()
                .SingleOrDefaultAsync(c => c.Id == order.CustomerId, token);
            
            return customer != null
                ? Result.Success(customer.DeliveryAddress)
                : Result.Failure<DeliveryAddress>(ErrorKind.BadRequest,
                    "customer.not.found");
        }
        catch (Exception e)
        {
            return Result.Failure<DeliveryAddress>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}