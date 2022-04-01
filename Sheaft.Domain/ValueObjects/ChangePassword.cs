namespace Sheaft.Domain.AccountManagement;

public record ChangePassword(string OldPassword, string NewPassword, string Confirm);