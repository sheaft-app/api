namespace Sheaft.Domain;

public record ChangePassword(string OldPassword, string NewPassword, string Confirm);