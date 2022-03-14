namespace Sheaft.Application;

public interface IMailerSettings
{
    string ApiId { get; set; }
    string ApiKey { get; set; }
    MailerUser Sender { get; set; }
    string Bounces { get; set; }
    bool SkipSending { get; set; }
    
    public class MailerUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}