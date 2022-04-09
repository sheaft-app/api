namespace Sheaft.Application.OrderManagement;

public record ProductQuantityDto(string ProductIdentifier, int Quantity);
public record DeliveryLineDto(string ProductIdentifier, int Quantity, IEnumerable<string> BatchIdentifiers);