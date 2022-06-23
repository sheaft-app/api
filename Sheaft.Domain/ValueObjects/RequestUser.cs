namespace Sheaft.Domain;

public record RequestUser(bool IsAuthenticated, ProfileKind? Kind = null, AccountId AccountId = null, string ProfileId = null);