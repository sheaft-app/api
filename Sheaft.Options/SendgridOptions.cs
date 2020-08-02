namespace Sheaft.Options
{
    public class SendgridOptions
    {
        public const string SETTING = "Sendgrid";
        public string ApiKey { get; set; }
        public string Url { get; set; }
        public SendgridSender Sender { get; set; }
        public SendgridGroups Groups { get; set; }
    }

    public class SendgridSender
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class SendgridGroups
    {
        public string Newsletter { get; set; }
    }
}
