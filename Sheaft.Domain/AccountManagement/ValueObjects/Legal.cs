namespace Sheaft.Domain.AccountManagement;

public record Legal
{
    private Legal(){}

    public Legal(LegalName name, Siret siret, Address address)
    {
        Name = name;
        Siret = siret;
        Address = address;
    }
    
    public LegalName Name{ get; }
    public Siret Siret{ get; }
    public Address Address { get; }
}