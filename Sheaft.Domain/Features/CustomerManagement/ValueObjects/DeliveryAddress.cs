namespace Sheaft.Domain.CustomerManagement;

public record DeliveryAddress(string Street, string? Complement, string Postcode, string City) 
    : Address(Street, Complement, Postcode, City);