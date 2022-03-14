using Sheaft.Application;

namespace Sheaft.Infrastructure;

public class MailerSettings : IMailerSettings
{
        public const string SETTING = "Mailer";
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public IMailerSettings.MailerUser Sender { get; set; }
        public string Bounces { get; set; }
        public bool SkipSending { get; set; } = false;
    }

