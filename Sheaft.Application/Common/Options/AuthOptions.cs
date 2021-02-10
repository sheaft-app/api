using System.Collections.Generic;

namespace Sheaft.Application.Common.Options
{
    public class AuthOptions
    {
        public const string SETTING = "Auth";
        public string Scheme { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public bool Caching { get; set; }
        public int CacheDurationInSeconds { get; set; }
        public AuthActions Actions { get; set; }
        public AuthClient App { get; set; }
        public AuthClient Manage { get; set; }
        public AuthClient Jobs { get; set; }
        public bool ValidateIssuer { get; set; } = true;
        public IEnumerable<string> ValidIssuers { get; set; }

    }

    public class AuthClient
    {
        public string Id { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
    }

    public class AuthActions
    {
        public string Profile { get; set; }
        public string Picture { get; set; }
        public string Delete { get; set; }
    }
}
