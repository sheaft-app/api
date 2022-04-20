using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.BillingManagement;

internal class RetrieveBillingInformation : IRetrieveBillingInformation
{
    private readonly IDbContext _context;

    public RetrieveBillingInformation(IDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<CustomerBillingInformation>> GetCustomerBilling(CustomerId customerIdentifier, CancellationToken token)
    {
        var customer = await _context.Set<Customer>()
            .SingleOrDefaultAsync(c => c.Identifier == customerIdentifier, token);

        if (customer == null)
            return Result.Failure<CustomerBillingInformation>(ErrorKind.NotFound, "billing.information.customer.not.found");
        
        var billingInformation = new CustomerBillingInformation(customerIdentifier, customer.BillingAddress.Name, customer.BillingAddress.Email, customer.Legal.Siret, BillingAddress.Copy(customer.BillingAddress));

        return Result.Success(billingInformation);
    }
    
    public async Task<Result<SupplierBillingInformation>> GetSupplierBilling(SupplierId supplierIdentifier, CancellationToken token)
    {
        var supplier = await _context.Set<Supplier>()
            .SingleOrDefaultAsync(c => c.Identifier == supplierIdentifier, token);

        if (supplier == null)
            return Result.Failure<SupplierBillingInformation>(ErrorKind.NotFound, "billing.information.supplier.not.found");
        
        var billingInformation = new SupplierBillingInformation(supplierIdentifier, supplier.BillingAddress.Name, supplier.BillingAddress.Email, supplier.Legal.Siret, BillingAddress.Copy(supplier.BillingAddress));

        return Result.Success(billingInformation);
    }
}