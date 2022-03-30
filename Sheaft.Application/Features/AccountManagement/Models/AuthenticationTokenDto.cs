namespace Sheaft.Application.AccountManagement;

public record AuthenticationTokenDto(string AccessToken, string RefreshToken, string TokenType, int ExpiresIn);