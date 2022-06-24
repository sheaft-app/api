using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record DeliveryLineQuantityDto(string ProductIdentifier, int Quantity, IEnumerable<string> BatchIdentifiers);

public record DeliveryLineDto(DeliveryLineKind Kind, string Identifier, string Name, string Code, int Quantity, decimal Vat, decimal UnitPrice, decimal TotalWholeSalePrice, decimal TotalVatPrice, decimal TotalOnSalePrice);