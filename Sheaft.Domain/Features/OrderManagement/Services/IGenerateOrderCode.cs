namespace Sheaft.Domain.OrderManagement;

public interface IGenerateOrderCode
{
    Task<Result<OrderCode>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}