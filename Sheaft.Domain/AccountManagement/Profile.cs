using System;
using Sheaft.Domain.AccountManagement.ValueObjects;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.AccountManagement;

public class Profile : Entity
{
    public Profile(CompanyName name, EmailAddress contactEmail, PhoneNumber contactPhone, Legal legal)
    {
        Name = name;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Legal = legal;
    }

    public CompanyName Name { get; set;}
    public EmailAddress ContactEmail { get; set; }
    public PhoneNumber ContactPhone { get; set;}
    public Legal Legal { get; set;}
    public Uri? Picture { get; set; }
    public CompanyDescription? Description { get; set; }
}

public record CompanyDescription(string Value);
public record Legal(LegalName Name, Siret Siret, Address.Address Address);

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