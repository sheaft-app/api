namespace Sheaft.Domain.AccountManagement;

public record PasswordReset(string AccountIdentifier) : DomainEvent;