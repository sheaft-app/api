using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;

namespace Sheaft.Infrastructure.OrderManagement;

internal class GenerateOrderCode : IGenerateOrderCode
{
    private int code = 0;
    public Task<Result<OrderCode>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token)
    {
        code++;
        return Task.FromResult(Result.Success(new OrderCode(code.ToString("0000000"))));
    }
}