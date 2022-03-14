namespace Sheaft.Domain.ProfileManagement;

public class Profile : AggregateRoot
{
    private Profile()
    {
    }

    private Profile(CompanyName name, EmailAddress contactEmail, PhoneNumber contactPhone, Legal legal, UserInfo userInfo)
    {
        Name = name;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Legal = legal;
        User = userInfo;
    }

    public static Profile Create(CompanyName name, EmailAddress contactEmail, PhoneNumber contactPhone, Legal legal, UserInfo userInfo)
    {
        return new Profile(name, contactEmail, contactPhone, legal, userInfo);
    }
    
    public ProfileId Identifier { get; set;}
    public CompanyName Name { get; set;}
    public EmailAddress ContactEmail { get; set; }
    public PhoneNumber ContactPhone { get; set;}
    public UserInfo User { get; set; }
    public Legal Legal { get; set;}
    public Uri? Picture { get; set; }
    public CompanyDescription? Description { get; set; }
}

public record CompanyDescription(string Value);
public record Legal(LegalName Name, Siret Siret, Address Address);
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