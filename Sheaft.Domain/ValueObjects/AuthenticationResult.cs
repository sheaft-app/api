namespace Sheaft.Domain;

public record AuthenticationResult(string AccessToken, string RefreshToken, string TokenType, int ExpiresIn);