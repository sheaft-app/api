namespace Sheaft.Domain;

public record BillingAddress(string Street, string? Complement, string Postcode, string City) 
    : Address(Street, Complement, Postcode, City)
{
    public static BillingAddress Copy(BillingAddress address)
    {
        return new BillingAddress(address.Street, address.Complement, address.Postcode, address.City);
    }
}