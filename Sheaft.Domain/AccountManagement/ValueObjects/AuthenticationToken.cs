namespace Sheaft.Domain.AccountManagement;

public record AuthenticationToken(string AccessToken, string RefreshToken, string TokenType, int ExpiresIn);