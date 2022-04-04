namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveDeliveryDays
{
    Task<Result<Maybe<IEnumerable<DeliveryDay>>>> ForAgreementBetween(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token);
}