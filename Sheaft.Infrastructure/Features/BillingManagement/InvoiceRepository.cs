using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.InvoiceManagement;

internal class InvoiceRepository : Repository<Invoice, InvoiceId>, IInvoiceRepository
{
    public InvoiceRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Invoice>> Get(InvoiceId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Invoice>(ErrorKind.NotFound, "invoice.not.found");
        });
    }

    public Task<Result<Maybe<Invoice>>> GetInvoiceWithCreditNote(InvoiceId creditNoteIdentifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.CreditNotes.Any(cn => cn.InvoiceIdentifier == creditNoteIdentifier), token);

            return Result.Success(result != null ? Maybe.From(result) : Maybe<Invoice>.None);
        });
    }
}