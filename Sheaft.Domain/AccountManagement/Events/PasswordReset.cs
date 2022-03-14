namespace Sheaft.Domain.AccountManagement;

public record PasswordReset(string Username) : DomainEvent;