namespace Sheaft.Domain.AccountManagement;

public record EmailChanged(string Username, string OldEmail, string NewEmail) : DomainEvent;