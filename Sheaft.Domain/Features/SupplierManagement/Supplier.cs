namespace Sheaft.Domain.SupplierManagement;

public class Supplier : AggregateRoot
{
    private Supplier()
    {
    }

    public Supplier(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, AccountId accountId, ShippingAddress? shippingAddress = null,
        BillingAddress? billingAddress = null)
    {
        Id = SupplierId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        ShippingAddress = shippingAddress ?? new ShippingAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        BillingAddress = billingAddress ?? new BillingAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountId = accountId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public SupplierId Id { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; }
    public BillingAddress BillingAddress { get; private set; }
    public AccountId AccountId { get; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }

    public void SetInfo(TradeName name, EmailAddress email, PhoneNumber phone)
    {
        TradeName = name;
        Email = email;
        Phone = phone;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public void SetShippingAddress(ShippingAddress shippingAddress)
    {
        ShippingAddress = shippingAddress;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public void SetBillingAddress(BillingAddress billingAddress)
    {
        BillingAddress = billingAddress;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public void SetLegal(CorporateName name, Siret siret, LegalAddress address)
    {
        Legal = new Legal(name, siret, address);
        UpdatedOn = DateTimeOffset.UtcNow;
    }
}