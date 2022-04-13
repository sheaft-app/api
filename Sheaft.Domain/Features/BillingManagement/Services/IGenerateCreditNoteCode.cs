namespace Sheaft.Domain.InvoiceManagement;

public interface IGenerateCreditNoteCode
{
    Task<Result<InvoiceReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}