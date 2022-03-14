namespace Sheaft.Domain;

public record RequestUser(bool IsAuthenticated, string? Identifier = null);