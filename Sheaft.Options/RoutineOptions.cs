using Sheaft.Core.Extensions;

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
    }
}
