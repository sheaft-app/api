namespace Sheaft.Domain.Enum
{
    public enum PurchaseOrderStatus
    {
        Waiting = 1,
        Accepted,
        Processing,
        Completed,
        Delivered = 6,
        Refused,
        Cancelled,
        Withdrawned,
        Expired
    }
}