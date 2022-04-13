namespace Sheaft.Domain.InvoiceManagement;

public interface IGenerateInvoiceCode
{
    Task<Result<InvoiceReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}