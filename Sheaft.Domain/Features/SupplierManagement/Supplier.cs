namespace Sheaft.Domain.SupplierManagement;

public class Supplier : AggregateRoot
{
    private Supplier()
    {
    }

    public Supplier(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, AccountId accountIdentifier, ShippingAddress? shippingAddress = null,
        BillingAddress? billingAddress = null)
    {
        Identifier = SupplierId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        ShippingAddress = shippingAddress ?? new ShippingAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        BillingAddress = billingAddress ?? new BillingAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountIdentifier = accountIdentifier;
    }

    public SupplierId Identifier { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; }
    public BillingAddress BillingAddress { get; private set; }
    public AccountId AccountIdentifier { get; }

    public Result SetInfo(TradeName name, EmailAddress email, PhoneNumber phone)
    {
        TradeName = name;
        Email = email;
        Phone = phone;
        return Result.Success();
    }

    public Result SetShippingAddress(ShippingAddress shippingAddress)
    {
        ShippingAddress = shippingAddress;
        return Result.Success();
    }

    public Result SetBillingAddress(BillingAddress billingAddress)
    {
        BillingAddress = billingAddress;
        return Result.Success();
    }

    public Result SetLegal(CorporateName name, Siret siret, LegalAddress address)
    {
        Legal = new Legal(name, siret, address);
        return Result.Success();
    }
}