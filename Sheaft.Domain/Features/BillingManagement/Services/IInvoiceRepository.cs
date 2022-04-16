namespace Sheaft.Domain.BillingManagement;

public interface IInvoiceRepository : IRepository<Invoice, InvoiceId>
{
    Task<Result<Maybe<Invoice>>> GetInvoiceWithCreditNote(InvoiceId creditNoteIdentifier, CancellationToken token);
}