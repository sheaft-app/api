namespace Sheaft.Application.ProductManagement;

public record OrderableProductDto(string Id, string Name, string Code, decimal UnitPrice, decimal Vat, DateTimeOffset UpdatedOn, string SupplierName, string SupplierId);