using System.Diagnostics.CodeAnalysis;
#pragma warning disable CS8618

namespace Sheaft.Application;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public interface IEmailingSettings
{
    string ApiId { get; set; }
    string ApiKey { get; set; }
    MailerUser Sender { get; set; }
    string Bounces { get; set; }
    bool SkipSending { get; set; }
    
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class MailerUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}