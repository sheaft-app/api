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