namespace Sheaft.Domain.AccountManagement;

public record EmailChanged(string AccountIdentifier, string OldEmail, string NewEmail) : DomainEvent;