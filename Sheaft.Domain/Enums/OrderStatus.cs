namespace Sheaft.Domain.OrderManagement;

public enum OrderStatus
{
    Draft,
    Pending,
    Accepted,
    Fulfilled,
    Delivered,
    Billed,
    Completed,
    Refused,
    Cancelled
}