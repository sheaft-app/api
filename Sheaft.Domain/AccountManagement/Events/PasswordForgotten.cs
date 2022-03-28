namespace Sheaft.Domain.AccountManagement;

public record PasswordForgotten(string AccountIdentifier, string Token, DateTimeOffset ExpiresOn) : DomainEvent;