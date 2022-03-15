namespace Sheaft.Application.AccountManagement;

public record ProfileDto(string Id, string Firstname, string Lastname, string Email, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn);