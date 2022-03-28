using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application;

public interface IUnitOfWork
{
    public IAccountRepository Accounts { get; }
    
    public Task<Result<int>> Save(CancellationToken token);
}