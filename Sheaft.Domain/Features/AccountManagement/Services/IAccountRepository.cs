namespace Sheaft.Domain.AccountManagement;

public interface IAccountRepository : IRepository<Account, AccountId>
{
    Task<Result<Account>> Get(Username username, CancellationToken token);
    Task<Result<Maybe<Account>>> Find(EmailAddress email, CancellationToken token);
}