namespace Sheaft.Domain;

public record AccessToken(string Value, string TokenType, int ExpiresIn);