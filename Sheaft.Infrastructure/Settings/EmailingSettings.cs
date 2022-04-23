using Sheaft.Application;
#pragma warning disable CS8618

namespace Sheaft.Infrastructure;

public class EmailingSettings : IEmailingSettings
{
        public const string SETTING = "Emailing";
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public IEmailingSettings.MailerUser Sender { get; set; }
        public string Bounces { get; set; }
        public bool SkipSending { get; set; } = false;
    }

