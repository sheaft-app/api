namespace Sheaft.Domain;

public record Address
{
    protected Address(){}
    public Address(string Street, string? Complement, string Postcode, string City)
    {
        this.Street = Street;
        this.Complement = Complement;
        this.Postcode = Postcode;
        this.City = City;
    }

    public string Street { get; init; }
    public string? Complement { get; init; }
    public string Postcode { get; init; }
    public string City { get; init; }

    public void Deconstruct(out string Street, out string? Complement, out string Postcode, out string City)
    {
        Street = this.Street;
        Complement = this.Complement;
        Postcode = this.Postcode;
        City = this.City;
    }

    public static Address Copy(Address address)
    {
        return new Address(address.Street, address.Complement, address.Postcode, address.City);
    }
}