namespace Sheaft.Options
{
    public class AuthOptions
    {
        public const string SETTING = "Auth";
        public string Scheme { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string ApiName { get; set; }
        public bool Caching { get; set; }
        public int CacheDurationInSeconds { get; set; }
        public AuthActions Actions { get; set; }
    }

    public class AuthActions
    {
        public string Profile { get; set; }
        public string Picture { get; set; }
        public string Delete { get; set; }
    }
}
