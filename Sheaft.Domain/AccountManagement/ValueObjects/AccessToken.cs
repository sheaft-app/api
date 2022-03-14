namespace Sheaft.Domain.AccountManagement;

public record AccessToken(string Value, string TokenType, int ExpiresIn);