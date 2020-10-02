namespace Sheaft.Options
{
    public class RoutineOptions
    {
        public const string SETTING = "Routines";
        public int CheckOrdersFromMinutes { get; set; } = 60;
        public int CheckOrderExpiredFromMinutes { get; set; } = 1440;
        public int CheckPayinsFromMinutes { get; set; } = 60;
        public int CheckPayinExpiredFromMinutes { get; set; } = 1440;
        public int CheckNewPayinRefundsFromMinutes { get; set; } = 10080;
        public int CheckPayinRefundsFromMinutes { get; set; } = 60;
        public int CheckPayinRefundExpiredFromMinutes { get; set; } = 1440;
        public int CheckNewPayoutsFromMinutes { get; set; } = 10080;
        public int CheckPayoutsFromMinutes { get; set; } = 60;
        public int CheckPayoutExpiredFromMinutes { get; set; } = 1440;
        public int CheckTransfersFromMinutes { get; set; } = 60;
        public int CheckTransferExpiredFromMinutes { get; set; } = 1440;
        public int CheckTransferRefundsFromMinutes { get; set; } = 60;
        public int CheckTransferRefundExpiredFromMinutes { get; set; } = 1440;
        public int CheckDonationsFromMinutes { get; set; } = 60;
        public int CheckDonationExpiredFromMinutes { get; set; } = 1440;
        public string CheckOrdersCron { get; set; } = "*/10 * * * *";
        public string CheckPayinsCron { get; set; } = "*/10 * * * *";
        public string CheckPayinRefundsCron { get; set; } = "*/10 * * * *";
        public string CheckNewPayinRefundsCron { get; set; } = "*/10 * * * *";
        public string CheckPayoutsCron { get; set; } = "*/10 * * * *";
        public string CheckNewPayoutsCron { get; set; } = "*/10 * * * *";
        public string CheckTransfersCron { get; set; } = "*/10 * * * *";
        public string CheckNewTransfersCron { get; set; } = "*/10 * * * *";
        public string CheckTransferRefundsCron { get; set; } = "*/10 * * * *";
        public string CheckNewTransferRefundsCron { get; set; } = "*/10 * * * *";
        public string CheckZonesProgressCron { get; set; } = "0 1 * * *";
        public string CheckZonesFileCron { get; set; } = "0 1 * * *";
    }
}
