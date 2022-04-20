namespace Sheaft.Domain.CustomerManagement;

public class Customer : AggregateRoot
{
    private Customer()
    {
    }

    public Customer(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, 
        AccountId accountIdentifier, DeliveryAddress? deliveryAddress = null, BillingAddress? billingAddress = null)
    {
        Identifier = CustomerId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        DeliveryAddress = deliveryAddress ?? new DeliveryAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        BillingAddress = billingAddress ?? new BillingAddress(tradeName.Value, email, legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountIdentifier = accountIdentifier;
    }

    public CustomerId Identifier { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public BillingAddress BillingAddress { get; private set; }
    public AccountId AccountIdentifier { get; }

    public Result SetInfo(TradeName name, EmailAddress email, PhoneNumber phone)
    {
        TradeName = name;
        Email = email;
        Phone = phone;

        return Result.Success();
    }

    public Result SetDeliveryAddress(DeliveryAddress deliveryAddress)
    {
        DeliveryAddress = deliveryAddress;
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