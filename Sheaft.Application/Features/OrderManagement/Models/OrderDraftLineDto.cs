using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderDraftLineDto(OrderLineKind Kind, string Identifier, string Name, string Code, int Quantity, decimal Vat, decimal UnitPrice);