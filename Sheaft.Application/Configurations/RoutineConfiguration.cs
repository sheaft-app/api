namespace Sheaft.Application.Configurations
{
    public class RoutineConfiguration
    {
        public const string SETTING = "Routines";
        public int CheckCartExpiredFromMinutes { get; set; } = 10080;
        
        public string CheckCartsCron { get; set; } = "0 1 * * *";
        public string CheckExpiredPurchaseOrdersCron { get; set; } = "30 1 * * *";
        public string CheckDeliveryBatchsCron { get; set; } = "15 1 * * *";
    }
}
