namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveReturnedReturnables
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(SupplierId supplierIdentifier, IEnumerable<ReturnedReturnable> returnedReturnables, CancellationToken token);
}