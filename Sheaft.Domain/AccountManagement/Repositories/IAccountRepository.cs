namespace Sheaft.Domain.AccountManagement;

public interface IAccountRepository : IRepository<Account, Username>
{
    Task<Result<Maybe<Account>>> FindByEmail(EmailAddress email, CancellationToken token);
}