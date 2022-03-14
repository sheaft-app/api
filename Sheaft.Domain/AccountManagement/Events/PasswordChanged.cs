namespace Sheaft.Domain.AccountManagement;

public record PasswordChanged(string Username) : DomainEvent;