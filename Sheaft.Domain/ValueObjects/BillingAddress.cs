namespace Sheaft.Domain;

public record BillingAddress(string Street, string? Complement, string Postcode, string City) 
    : Address(Street, Complement, Postcode, City);