namespace Sheaft.Domain.AccountManagement;

public interface ISecurityTokensProvider
{
    AccessToken GenerateAccessToken(Account account);
    (RefreshToken data, string token) GenerateRefreshToken(Username username);
    (Username, RefreshTokenId) ReadRefreshTokenData(string refreshToken);
}