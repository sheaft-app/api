using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Infrastructure.OrderManagement;

internal class GenerateDeliveryCode : IGenerateDeliveryCode
{
    private int code = 0;
    public Task<Result<DeliveryReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token)
    {
        code++;
        return Task.FromResult(Result.Success(new DeliveryReference(code.ToString("0000000"))));
    }
}