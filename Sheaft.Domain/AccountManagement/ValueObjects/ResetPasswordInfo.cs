namespace Sheaft.Domain.AccountManagement;

public record ResetPasswordInfo(string Token, DateTimeOffset ExpiresOn);