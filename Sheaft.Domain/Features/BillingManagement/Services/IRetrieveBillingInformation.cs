namespace Sheaft.Domain.InvoiceManagement;

public interface IRetrieveBillingInformation
{
    Task<Result<CustomerBillingInformation>> GetCustomerBilling(CustomerId customerIdentifier, CancellationToken token);
    Task<Result<SupplierBillingInformation>> GetSupplierBilling(SupplierId supplierIdentifier, CancellationToken token);
}