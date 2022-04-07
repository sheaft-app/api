namespace Sheaft.Domain.OrderManagement;

public interface IGenerateOrderCode
{
    Task<Result<OrderReference>> GenerateNextCode(SupplierId supplierIdentifier, CancellationToken token);
}