namespace Sheaft.Domain.SupplierManagement;

public class Supplier : AggregateRoot
{
    private Supplier()
    {
    }

    public Supplier(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, ShippingAddress? shippingAddress,
        AccountId accountIdentifier)
    {
        Identifier = SupplierId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        ShippingAddress = shippingAddress ?? new ShippingAddress(legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountIdentifier = accountIdentifier;
    }

    public SupplierId Identifier { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; }
    public AccountId AccountIdentifier { get; }

    public void SetInfo(TradeName name, EmailAddress email, PhoneNumber phone, ShippingAddress address)
    {
        TradeName = name;
        Email = email;
        Phone = phone;
        ShippingAddress = address;
    }

    public void SetLegal(CorporateName name, Siret siret, LegalAddress address)
    {
        Legal = new Legal(name, siret, address);
    }
}