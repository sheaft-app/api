namespace Sheaft.Domain.BillingManagement;

public interface IGenerateCreditNoteCode
{
    Result<CreditNoteReference> GenerateNextCode(SupplierId supplierIdentifier);
}