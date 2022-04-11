namespace Sheaft.Domain;

public class Customer : AggregateRoot
{
    private Customer()
    {
    }

    public Customer(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, DeliveryAddress? deliveryAddress,
        AccountId accountIdentifier)
    {
        Identifier = CustomerId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        DeliveryAddress = deliveryAddress ?? new DeliveryAddress(legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountIdentifier = accountIdentifier;
    }

    public CustomerId Identifier { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public AccountId AccountIdentifier { get; }

    public Result SetInfo(TradeName name, EmailAddress email, PhoneNumber phone, DeliveryAddress address)
    {
        TradeName = name;
        Email = email;
        Phone = phone;
        DeliveryAddress = address;

        return Result.Success();
    }

    public Result SetLegal(CorporateName name, Siret siret, LegalAddress address)
    {
        Legal = new Legal(name, siret, address);
        return Result.Success();
    }
}