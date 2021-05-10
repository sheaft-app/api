namespace Sheaft.Options
{
    public class MailerOptions
    {
        public const string SETTING = "Mailer";
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public MailerUser Sender { get; set; }
        public string Bounces { get; set; }
        public bool SkipSending { get; set; } = false;
    }

    public class MailerUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
