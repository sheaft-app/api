using Sheaft.Core.Extensions;

namespace Sheaft.Options
{
    public class PspOptions
    {
        public const string SETTING = "Psp";
        public string ApiUrl { get; set; }
        public string AppRedirectUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string PaymentUrl { get; set; }
        public string PaylineUrl { get; set; }
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal Percent { get; set; }
        public string Token => $"{ClientId}:{ApiKey}".Base64Encode();
    }
}
