namespace Sheaft.Domain.Enum
{
    public enum PurchaseOrderStatus
    {
        Waiting = 1,
        Accepted,
        Processing,
        Completed,
        Shipping,
        Delivered,
        Refused,
        Cancelled,
        Withdrawned,
        Expired
    }
}