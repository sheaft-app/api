using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Application;

public interface IUnitOfWork
{
    public IAccountRepository Accounts { get; }
    
    public ISupplierRepository Suppliers { get; }
    
    public ICatalogRepository Catalogs { get; }
    public IProductRepository Products { get; }
    
    public Task<Result<int>> Save(CancellationToken token);
}