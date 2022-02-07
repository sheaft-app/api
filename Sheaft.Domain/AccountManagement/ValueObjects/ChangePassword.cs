namespace Sheaft.Domain.AccountManagement.ValueObjects;

public record ChangePassword(string OldPassword, NewPassword NewPassword);