namespace Sheaft.Domain;

public record BillingInformation
{
    private BillingInformation(){}
    
    public BillingInformation(TradeName Name, EmailAddress Email, Siret Siret, BillingAddress Address)
    {
        this.Name = Name;
        this.Email = Email;
        this.Siret = Siret;
        this.Address = Address;
    }

    public static BillingInformation Copy(BillingInformation billingInformation)
    {
        return new BillingInformation(billingInformation.Name, billingInformation.Email, billingInformation.Siret,
            BillingAddress.Copy(billingInformation.Address));
    }

    public TradeName Name { get; init; }
    public Siret Siret { get; init; }
    public BillingAddress Address { get; init; }
    public EmailAddress Email { get; set; }

    public void Deconstruct(out TradeName Name, out EmailAddress Email, out Siret Siret, out BillingAddress Address)
    {
        Name = this.Name;
        Email = this.Email;
        Siret = this.Siret;
        Address = this.Address;
    }
}