namespace Sheaft.Application.ProfileManagement;

public record ProfileDto(string Id, string Firstname, string Lastname, string Email, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn);