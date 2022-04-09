namespace Sheaft.Domain.OrderManagement;

public interface ICreateDeliveryReturnedReturnables
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(SupplierId supplierIdentifier, IEnumerable<ReturnedReturnable> returnedReturnables, CancellationToken token);
}