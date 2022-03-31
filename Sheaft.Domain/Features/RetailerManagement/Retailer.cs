﻿namespace Sheaft.Domain.RetailerManagement;

public class Retailer : AggregateRoot
{
    private Retailer()
    {
    }

    public Retailer(TradeName tradeName, EmailAddress email, PhoneNumber phone, Legal legal, DeliveryAddress? deliveryAddress,
        AccountId accountIdentifier)
    {
        Identifier = RetailerId.New();
        TradeName = tradeName;
        Legal = legal;
        Email = email;
        Phone = phone;
        DeliveryAddress = deliveryAddress ?? new DeliveryAddress(legal.Address.Street, legal.Address.Complement, legal.Address.Postcode, legal.Address.City);
        AccountIdentifier = accountIdentifier;
    }

    public RetailerId Identifier { get; }
    public TradeName TradeName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Legal Legal { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public AccountId AccountIdentifier { get; }

    public void SetInfo(TradeName name, EmailAddress email, PhoneNumber phone, DeliveryAddress address)
    {
        TradeName = name;
        Email = email;
        Phone = phone;
        DeliveryAddress = address;
    }

    public void SetLegal(CorporateName name, Siret siret, LegalAddress address)
    {
        Legal = new Legal(name, siret, address);
    }
}