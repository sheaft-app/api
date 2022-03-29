using Sheaft.Domain.AccountManagement;

namespace Sheaft.Domain.SupplierManagement;

public class Supplier : AggregateRoot
{
    private Supplier()
    {
    }

    public Supplier(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, Address? shippingAddress,
        AccountId accountIdentifier)
    {
        Identifier = SupplierId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        ShippingAddress = shippingAddress ?? new Address(legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountIdentifier = accountIdentifier;
    }

    public SupplierId Identifier { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public Address ShippingAddress { get; private set; }
    public AccountId AccountIdentifier { get; }
}

public record Legal
{
    private Legal(){}
    public Legal(CorporateName name, Siret siret, Address address)
    {
        CorporateName = name;
        Siret = siret;
        Address = address;
    }
    
    public CorporateName CorporateName { get; }
    public Siret Siret{ get; }
    public Address Address { get; }
}

public record CorporateName
{
    public CorporateName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }

    public string Value { get; }
}
public record TradeName
{
    public TradeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        Value = value;
    }

    public string Value { get; }
}