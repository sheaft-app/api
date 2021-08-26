namespace Sheaft.Core.Options
{
    public class SignalrOptions
    {
        public const string SETTING = "Signalr";
        public string Scheme { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string NotifyGroupUrl { get; set; }
        public string NotifyUserUrl { get; set; }
    }
}
