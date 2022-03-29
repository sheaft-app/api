using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.SupplierManagement;

namespace Sheaft.Application;

public interface IUnitOfWork
{
    public IAccountRepository Accounts { get; }
    public ISupplierRepository Suppliers { get; }
    
    public Task<Result<int>> Save(CancellationToken token);
}