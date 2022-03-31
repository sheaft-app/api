namespace Sheaft.Domain;

public record Legal
{
    private Legal(){}
    public Legal(CorporateName name, Siret siret, LegalAddress address)
    {
        CorporateName = name;
        Siret = siret;
        Address = address;
    }
    
    public CorporateName CorporateName { get; }
    public Siret Siret{ get; }
    public LegalAddress Address { get; }
}