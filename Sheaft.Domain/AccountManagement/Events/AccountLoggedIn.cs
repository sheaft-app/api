namespace Sheaft.Domain.AccountManagement;

public record AccountLoggedIn(string AccountId) : DomainEvent;