namespace Sheaft.Domain;

public record BillingInformation
{
    private BillingInformation(){}
    
    public BillingInformation(TradeName Name, Siret Siret, BillingAddress Address)
    {
        this.Name = Name;
        this.Siret = Siret;
        this.Address = Address;
    }

    public TradeName Name { get; init; }
    public Siret Siret { get; init; }
    public BillingAddress Address { get; init; }

    public void Deconstruct(out TradeName Name, out Siret Siret, out BillingAddress Address)
    {
        Name = this.Name;
        Siret = this.Siret;
        Address = this.Address;
    }
}