namespace Sheaft.Domain.Enums
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
        Cancelled
    }
}