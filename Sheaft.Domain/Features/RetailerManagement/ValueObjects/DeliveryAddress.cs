namespace Sheaft.Domain.RetailerManagement;

public record DeliveryAddress(string Street, string? Complement, string Postcode, string City) 
    : Address(Street, Complement, Postcode, City);