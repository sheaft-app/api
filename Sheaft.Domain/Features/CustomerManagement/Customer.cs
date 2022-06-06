namespace Sheaft.Domain.CustomerManagement;

public class Customer : AggregateRoot
{
    private Customer()
    {
    }

    public Customer(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, 
        AccountId accountId, DeliveryAddress? deliveryAddress = null, BillingAddress? billingAddress = null)
    {
        Id = CustomerId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        DeliveryAddress = deliveryAddress ?? new DeliveryAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        BillingAddress = billingAddress ?? new BillingAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountId = accountId;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public CustomerId Id { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
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

    public void SetDeliveryAddress(DeliveryAddress deliveryAddress)
    {
        DeliveryAddress = deliveryAddress;
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