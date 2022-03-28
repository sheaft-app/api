namespace Sheaft.Domain.AccountManagement;

public record AccountLoggedIn(string AccountIdentifier) : DomainEvent;