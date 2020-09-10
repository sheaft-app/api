using Sheaft.Core.Extensions;

namespace Sheaft.Options
{
    public class ApiOptions
    {
        public const string SETTING = "Api";
        public string Url { get; set; }
    }

    public class PspOptions
    {
        public const string SETTING = "Psp";
        public string ApiUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string PaiementUrl { get; set; }
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string Token
        {
            get
            {
                return $"{ClientId}:{ApiKey}".Base64Encode();
            }
        }
    }
}
