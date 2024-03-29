﻿namespace Sheaft.Core.Options
{
    public class RoutineOptions
    {
        public const string SETTING = "Routines";
        public int CheckOrderExpiredFromMinutes { get; set; } = 10080;
        
        public string CheckPayinsCron { get; set; } = "*/45 * * * *";
        public string CheckZonesProgressCron { get; set; } = "0 0 * * *";
        public string CheckZonesFileCron { get; set; } = "30 0 * * *";
        public string CheckProducersFileCron { get; set; } = "40 0 * * *";
        public string CheckStoresFileCron { get; set; } = "50 0 * * *";
        public string CheckOrdersCron { get; set; } = "0 1 * * *";
        public string CheckTransfersCron { get; set; } = "0 2 * * *";
        public string CheckPayoutsCron { get; set; } = "0 3 * * *";
        public string CheckDonationsCron { get; set; } = "30 4 * * *";
        public string CheckPayinRefundsCron { get; set; } = "0 5 * * *";
        public string CheckNewPayoutsCron { get; set; } = "30 5 * * MON";
        public string CheckExpiredPurchaseOrdersCron { get; set; } = "30 1 * * *";
        public string CheckPreAuthorizationsCron { get; set; } = "*/10 * * * *";
        public string CheckDeliveryBatchsCron { get; set; } = "15 1 * * *";
    }
}
