using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class CreateDeliveryReturnedReturnables : ICreateDeliveryReturnedReturnables
{
    private readonly IDbContext _context;

    public CreateDeliveryReturnedReturnables(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<DeliveryLine>>> Get(SupplierId supplierIdentifier, IEnumerable<ReturnedReturnable> returnedReturnables, CancellationToken token)
    {
        try
        {
            var returnedReturnableIdentifiers = returnedReturnables.Select(p => p.Identifier).ToList();
            var returnables = await _context
                .Set<Returnable>()
                .Where(r => r.SupplierIdentifier == supplierIdentifier && returnedReturnableIdentifiers.Contains(r.Identifier))
                .ToListAsync(token);
            
            if (returnables.Count != returnedReturnables.Count())
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.BadRequest,
                    "delivery.returnables.to.adjust.not.found");

            var adjustedReturnables = returnables.Select(p =>
                DeliveryLine.CreateReturnedReturnableLine(p.Identifier, p.Reference, p.Name,
                    GetProductQuantity(p.Identifier, returnedReturnables), p.UnitPrice,
                    p.Vat));

            if (adjustedReturnables.Any(r => r.Quantity.Value >= 0))
                return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.BadRequest,
                    "delivery.returnables.quantity.must.be.lower.than.zero");

            return Result.Success(adjustedReturnables);
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<DeliveryLine>>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }

    private Quantity GetProductQuantity(ReturnableId productIdentifier, IEnumerable<ReturnedReturnable> returnedReturnables)
    {
        return returnedReturnables.Single(p => p.Identifier == productIdentifier).Quantity;
    }
}