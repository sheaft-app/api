namespace Sheaft.Domain.Extensions
{
    public static class IdentifierExtensions
    {
        public static string AsReceiptIdentifier(this int identifier)
        {
            return $"BR{identifier:0000000}";
        }
        
        public static string AsDeliveryIdentifier(this int identifier)
        {
            return $"BL{identifier:0000000}";
        }
        
        public static string AsPurchaseOrderIdentifier(this int identifier)
        {
            return $"CD{identifier:0000000}";
        }
    }
}