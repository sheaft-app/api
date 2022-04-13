namespace Sheaft.Application.OrderManagement;

public record DeliveryLineDto(string ProductIdentifier, int Quantity, IEnumerable<string> BatchIdentifiers);