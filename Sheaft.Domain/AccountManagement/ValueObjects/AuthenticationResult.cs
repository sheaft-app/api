namespace Sheaft.Domain.AccountManagement;

public record AuthenticationResult(string AccessToken, string RefreshToken, string TokenType, int ExpiresIn);