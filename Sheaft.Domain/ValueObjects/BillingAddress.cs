namespace Sheaft.Domain;

public record BillingAddress : Address
{
    private BillingAddress(){}

    public BillingAddress(string Name, EmailAddress Email, string Street, string? Complement, string Postcode, string City) : base(Street, Complement, Postcode, City)
    {
        this.Name = Name;
        this.Email = Email;
    }

    public string Name { get; init; }
    public EmailAddress Email { get; init; }

    public static BillingAddress Copy(BillingAddress address)
    {
        return new BillingAddress(address.Name, address.Email, address.Street, address.Complement, address.Postcode, address.City);
    }

    public void Deconstruct(out string Name, out EmailAddress Email, out string Street, out string? Complement, out string Postcode, out string City)
    {
        Name = this.Name;
        Email = this.Email;
        Street = this.Street;
        Complement = this.Complement;
        Postcode = this.Postcode;
        City = this.City;
    }
}