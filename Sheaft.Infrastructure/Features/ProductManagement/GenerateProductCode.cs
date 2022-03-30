using Sheaft.Domain;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.ProductManagement;

internal class GenerateProductCode : IGenerateProductCode
{
    private static int code = 0;
    public Task<Result<ProductCode>> GenerateNextProductCode(SupplierId supplierIdentifier, CancellationToken token)
    {
        code++;
        return Task.FromResult(Result.Success(new ProductCode(code.ToString())));
    }
}