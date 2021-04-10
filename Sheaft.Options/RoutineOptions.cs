using System;

namespace Sheaft.Options
{
    public class RoutineOptions
    {
        public const string SETTING = "Routines";
        public int CheckOrderExpiredFromMinutes { get; set; } = 10080;
        
        public string CheckPayinsCron { get; set; } = "*/15 * * * *";
        public string CheckZonesProgressCron { get; set; } = "0 0 * * *";
        public string CheckZonesFileCron { get; set; } = "30 0 * * *";
        public string CheckProducersFileCron { get; set; } = "45 0 * * *";
        public string CheckOrdersCron { get; set; } = "0 1 * * *";
        public string CheckTransfersCron { get; set; } = "0 2 * * *";
        public string CheckPayoutsCron { get; set; } = "0 3 * * *";
        public string CheckDonationsCron { get; set; } = "30 4 * * *";
        public string CheckPayinRefundsCron { get; set; } = "0 5 * * *";
        public string CheckNewPayoutsCron { get; set; } = "30 5 * * MON";
        public string CheckExpiredPurchaseOrdersCron { get; set; } = "30 1 * * *";
        public string CheckPreAuthorizationsCron { get; set; } = "*/15 * * * *";
        public string CheckNewPreAuthorizedPayinCron { get; set; } = "0 6 * * *";
    }
}
