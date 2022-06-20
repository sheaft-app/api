using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderLineDto(OrderLineKind Kind, string Identifier, string Name, string Code, int Quantity, decimal Vat, decimal UnitPrice, decimal TotalWholeSalePrice, decimal TotalVatPrice, decimal TotalOnSalePrice);