namespace Sheaft.Domain.AccountManagement;

public class Profile : AggregateRoot
{
    private Profile()
    {
    }

    public Profile(CompanyName name, EmailAddress contactEmail, PhoneNumber contactPhone, Legal legal, UserInfo userInfo)
    {
        Identifier = ProfileId.New();
        Name = name;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Legal = legal;
        User = userInfo;
    }
    
    public ProfileId Identifier { get; set;}
    public CompanyName Name { get; set;}
    public EmailAddress ContactEmail { get; set; }
    public PhoneNumber ContactPhone { get; set;}
    public UserInfo User { get; set; }
    public Legal Legal { get; set;}
}

public record ProfileDescription(string Value);

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
public record UserInfo(string Firstname, string Lastname);

public record Social
{
    public Social()
    {
    }
    
    public Social(Uri? facebook, Uri? instagram, Uri? twitter, Uri? website)
    {
        Facebook = facebook;
        Instagram = instagram;
        Twitter = twitter;
        Website = website;
    }

    public Uri? Facebook { get; set; }
    public Uri? Instagram { get; set; }
    public Uri? Twitter { get; set; }
    public Uri? Website { get; set; }
}