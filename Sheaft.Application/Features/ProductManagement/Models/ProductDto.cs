namespace Sheaft.Application.ProductManagement;

public record ProductDto(string Id, string Name, string Code, decimal UnitPrice, decimal Vat, DateTimeOffset UpdatedOn);