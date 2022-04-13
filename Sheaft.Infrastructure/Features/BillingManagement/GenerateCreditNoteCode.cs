using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Infrastructure.InvoiceManagement;

internal class GenerateCreditNoteCode : IGenerateCreditNoteCode
{
    private int code = 0;
    public Task<Result<InvoiceReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token)
    {
        code++;
        return Task.FromResult(Result.Success(new InvoiceReference(code.ToString("0000000"))));
    }
}