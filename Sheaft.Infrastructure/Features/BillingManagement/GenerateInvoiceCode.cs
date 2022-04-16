using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Infrastructure.BillingManagement;

internal class GenerateInvoiceCode : IGenerateInvoiceCode
{
    private int code = 0;
    public Task<Result<InvoiceReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token)
    {
        code++;
        return Task.FromResult(Result.Success(new InvoiceReference(code.ToString("0000000"))));
    }
}