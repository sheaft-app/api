using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.BillingManagement;

internal class GenerateCreditNoteCode : IGenerateCreditNoteCode
{
    private static readonly object Locker = new { };
    private readonly IDbContext _context;
    private CreditNoteReference? _currentReference;

    public GenerateCreditNoteCode(IDbContext context)
    {
        _context = context;
    }

    public Result<CreditNoteReference> GenerateNextCode(SupplierId supplierIdentifier)
    {
        lock (Locker)
        {
            _currentReference ??= _context.Set<Invoice>()
                .Where(o => o.Supplier.Identifier == supplierIdentifier && o.Reference != null && o.Kind ==InvoiceKind.CreditNote)
                .OrderByDescending(o => o.Reference)
                .Select(o => new CreditNoteReference(o.Reference.Value))
                .FirstOrDefault() ?? new CreditNoteReference(0);

            var nextResult = CreditNoteReference.Next(_currentReference);
            if (nextResult.IsFailure)
                return nextResult;

            _currentReference = nextResult.Value;
            return Result.Success(_currentReference);
        }
    }
}