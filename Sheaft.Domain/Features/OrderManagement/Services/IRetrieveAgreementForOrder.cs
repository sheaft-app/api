namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveAgreementForOrder
{
    Task<Result<bool>> IsExistingBetweenSupplierAndCustomer(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        CancellationToken token);
}