namespace Sheaft.Domain;

public record ResetPasswordInfo(string Token, DateTimeOffset CreatedOn, DateTimeOffset ExpiresOn);