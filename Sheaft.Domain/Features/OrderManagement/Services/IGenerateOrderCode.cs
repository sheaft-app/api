namespace Sheaft.Domain.OrderManagement;

public interface IGenerateOrderCode
{
    Task<Result<OrderCode>> GenerateNextOrderCode(SupplierId supplierIdentifier, CancellationToken token);
}