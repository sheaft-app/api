namespace Sheaft.Domain;

public interface IRetrieveProfile
{
    Task<Result<Maybe<Profile>>> GetAccountProfile(AccountId identifier, CancellationToken token);
}