namespace Sheaft.Application.ProductManagement;

public record ReturnableDto(string Id, string Name, string Code, decimal UnitPrice, decimal Vat, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn);