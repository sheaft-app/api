using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Application;

public interface IUnitOfWork
{
    public IAccountRepository Accounts { get; }
    public IProfileRepository Profiles { get; }
    
    void Add<T>(T entity) where T :class;
    void Update<T>(T entity) where T :class;
    void Remove<T>(T entity) where T :class;
    public Task<Result<int>> Save(CancellationToken token);
}