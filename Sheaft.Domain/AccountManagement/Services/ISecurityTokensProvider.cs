namespace Sheaft.Domain.AccountManagement;

public interface ISecurityTokensProvider
{
    AccessToken GenerateAccessToken(Account account);
    RefreshToken GenerateRefreshToken(Username username);
    (Username, RefreshTokenId) RetrieveTokenIdentifierData(string refreshToken);
}