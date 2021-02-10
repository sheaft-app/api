using Sheaft.Application.Common.Extensions;

namespace Sheaft.Application.Common.Options
{
    public class PspOptions
    {
        public const string SETTING = "Psp";
        public string ApiUrl { get; set; }
        public string AppRedirectPendingUrl { get; set; }
        public string AppRedirectSuccessUrl { get; set; }
        public string AppRedirectFailedUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string PaymentUrl { get; set; }
        public string PaylineUrl { get; set; }
        public string ClientId { get; set; }
        public string UserId { get; set; }
        public string WalletId { get; set; }
        public string DocumentWalletId { get; set; }
        public string ApiKey { get; set; }
        public decimal FixedAmount { get; set; } = 0.18m;
        public decimal Percent { get; set; } = 0.018m;
        public decimal VatPercent { get; set; } = 0.17m;
        public decimal ProducerFees { get; set; } = 2.93m;
        public bool SkipIdempotencyKey { get; set; } = false;
        public string Token => $"{ClientId}:{ApiKey}".Base64Encode();
    }
}
