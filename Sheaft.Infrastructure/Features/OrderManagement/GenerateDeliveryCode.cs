using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

internal class GenerateDeliveryCode : IGenerateDeliveryCode
{
    private static readonly object Locker = new { };
    private readonly IDbContext _context;
    private DeliveryReference? _currentReference;

    public GenerateDeliveryCode(IDbContext context)
    {
        _context = context;
    }

    public Result<DeliveryReference> GenerateNextCode(SupplierId supplierIdentifier)
    {
        lock (Locker)
        {
            _currentReference ??= _context.Set<Delivery>()
                .Where(o => o.SupplierIdentifier == supplierIdentifier && o.Reference != null)
                .OrderByDescending(o => o.Reference)
                .Select(o => o.Reference)
                .FirstOrDefault() ?? new DeliveryReference(0);

            var nextResult = DeliveryReference.Next(_currentReference);
            if (nextResult.IsFailure)
                return nextResult;

            _currentReference = nextResult.Value;
            return Result.Success(_currentReference);
        }
    }
}