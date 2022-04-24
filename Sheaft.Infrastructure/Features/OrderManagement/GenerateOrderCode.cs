using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

internal class GenerateOrderCode : IGenerateOrderCode
{
    private static readonly object Locker = new { };
    private readonly IDbContext _context;
    private OrderReference? _currentReference;

    public GenerateOrderCode(IDbContext context)
    {
        _context = context;
    }

    public Result<OrderReference> GenerateNextCode(SupplierId supplierIdentifier)
    {
        lock (Locker)
        {
            _currentReference ??= _context.Set<Order>()
                .Where(o => o.SupplierIdentifier == supplierIdentifier && o.Reference != null)
                .OrderByDescending(o => o.Reference)
                .Select(o => o.Reference)
                .FirstOrDefault() ?? new OrderReference(0);

            var nextResult = OrderReference.Next(_currentReference);
            if (nextResult.IsFailure)
                return nextResult;

            _currentReference = nextResult.Value;
            return Result.Success(_currentReference);
        }
    }
}