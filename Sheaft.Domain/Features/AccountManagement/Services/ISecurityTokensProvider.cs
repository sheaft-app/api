namespace Sheaft.Domain.AccountManagement;

public interface ISecurityTokensProvider
{
    AccessToken GenerateAccessToken(Account account, Profile? profile);
    RefreshToken GenerateRefreshToken(AccountId accountIdentifier);
    (AccountId, RefreshTokenId) RetrieveTokenIdentifierData(string refreshToken);
}