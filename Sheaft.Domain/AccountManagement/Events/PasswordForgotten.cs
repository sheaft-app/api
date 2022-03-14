namespace Sheaft.Domain.AccountManagement;

public record PasswordForgotten(string Username, string Token, DateTimeOffset ExpiresOn) : DomainEvent;