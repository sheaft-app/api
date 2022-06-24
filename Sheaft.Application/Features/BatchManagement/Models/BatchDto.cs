namespace Sheaft.Application.BatchManagement;

public record BatchDto(string Id, string Number, DateTimeKind Kind, DateOnly Date, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn);