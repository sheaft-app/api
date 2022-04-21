namespace Sheaft.Domain.AccountManagement;

public interface IRetrieveProfile
{
    Task<Result<Maybe<Profile>>> GetAccountProfile(AccountId identifier, CancellationToken token);
}