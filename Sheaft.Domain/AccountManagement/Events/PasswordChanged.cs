namespace Sheaft.Domain.AccountManagement;

public record PasswordChanged(string AccountIdentifier) : DomainEvent;