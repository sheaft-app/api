using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Application;

public interface IUnitOfWork
{
    public IAccountRepository Accounts { get; }
    
    public ISupplierRepository Suppliers { get; }
    
    public ICustomerRepository Customers { get; }
    
    public ICatalogRepository Catalogs { get; }
    public IProductRepository Products { get; }
    public IReturnableRepository Returnables { get; }
    
    public IAgreementRepository Agreements { get; }
    
    public IOrderRepository Orders { get; }
    public IDeliveryRepository Deliveries { get; }
    
    public IInvoiceRepository Invoices { get; }
    
    public IBatchRepository Batches { get; }
    
    public Task<Result<int>> Save(CancellationToken token);
}