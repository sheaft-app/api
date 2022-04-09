namespace Sheaft.Domain.OrderManagement;

public interface ICreateDeliveryLines
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(Delivery delivery, IEnumerable<DeliveryProductBatches> productLines, CancellationToken token);
}