namespace Sheaft.Domain;

public record Address
{
    public const int STREET_MAXLENGTH = 200;
    public const int COMPLEMENT_MAXLENGTH = 150;
    public const int POSTCODE_MAXLENGTH = 5;
    public const int CITY_MAXLENGTH = 80;
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