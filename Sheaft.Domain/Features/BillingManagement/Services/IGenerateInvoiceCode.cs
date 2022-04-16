namespace Sheaft.Domain.BillingManagement;

public interface IGenerateInvoiceCode
{
    Task<Result<InvoiceReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}