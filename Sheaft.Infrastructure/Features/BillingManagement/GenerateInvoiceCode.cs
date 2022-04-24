using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.BillingManagement;

internal class GenerateInvoiceCode : IGenerateInvoiceCode
{
    private static readonly object Locker = new { };
    private readonly IDbContext _context;
    private InvoiceReference? _currentReference;

    public GenerateInvoiceCode(IDbContext context)
    {
        _context = context;
    }

    public Result<InvoiceReference> GenerateNextCode(SupplierId supplierIdentifier)
    {
        lock (Locker)
        {
            _currentReference ??= _context.Set<Invoice>()
                .Where(o => o.Supplier.Identifier == supplierIdentifier && o.Reference != null && (o.Kind == InvoiceKind.Invoice || o.Kind == InvoiceKind.InvoiceCancellation))
                .OrderByDescending(o => o.Reference)
                .Select(o => new InvoiceReference(o.Reference.Value))
                .FirstOrDefault() ?? new InvoiceReference(0);

            var nextResult = InvoiceReference.Next(_currentReference);
            if (nextResult.IsFailure)
                return nextResult;

            _currentReference = nextResult.Value;
            return Result.Success(_currentReference);
        }
    }
}