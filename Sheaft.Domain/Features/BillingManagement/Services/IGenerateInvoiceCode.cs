namespace Sheaft.Domain.BillingManagement;

public interface IGenerateInvoiceCode
{
    Result<InvoiceReference> GenerateNextCode(SupplierId supplierIdentifier);
}