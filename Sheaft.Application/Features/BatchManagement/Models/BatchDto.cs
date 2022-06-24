namespace Sheaft.Application.BatchManagement;

public record BatchDto(string Id, string Number, DateTimeKind Kind, DateTime ExpirationDate, DateTime? ProductionDate, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn);