namespace Sheaft.Domain.BillingManagement;

public interface IGenerateCreditNoteCode
{
    Task<Result<InvoiceReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}