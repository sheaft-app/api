using Sheaft.Core.Extensions;
using System;

namespace Sheaft.Options
{

    public class RoutineOptions
    {
        public const string SETTING = "Routines";
        public int CheckOrdersFromMinutes { get; set; }
        public int CheckOrderExpiredFromMinutes { get; set; }
        public int CheckPayinsFromMinutes { get; set; }
        public int CheckPayinExpiredFromMinutes { get; set; }
        public int CheckNewPayinRefundsFromMinutes { get; set; }
        public int CheckPayinRefundsFromMinutes { get; set; }
        public int CheckPayinRefundExpiredFromMinutes { get; set; }
        public int CheckNewPayoutsFromMinutes { get; set; }
        public int CheckPayoutsFromMinutes { get; set; }
        public int CheckPayoutExpiredFromMinutes { get; set; }
        public int CheckNewTransfersFromMinutes { get; set; }
        public int CheckTransfersFromMinutes { get; set; }
        public int CheckTransferExpiredFromMinutes { get; set; }
        public int CheckNewTransferRefundsFromMinutes { get; set; }
        public int CheckTransferRefundsFromMinutes { get; set; }
        public int CheckTransferRefundExpiredFromMinutes { get; set; }
        public int CheckDonationsFromMinutes { get; set; }
        public int CheckDonationExpiredFromMinutes { get; set; }
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
        public string CheckZonesProgressCron { get; set; } = "*/10 * * * *";
        public string CheckZonesFileCron { get; set; } = "*/10 * * * *";
    }
}
