using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ShortDeliveryBatchViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Name { get; set; }
        public DeliveryBatchStatus Status { get; set; }
        public int ProductsToDeliverCount { get; set; }
        public int ProductsDeliveredCount { get; set; }
        public int DeliveriesCount { get; set; }
        public int ReturnablesCount { get; set; }
        public int ReturnedReturnablesCount { get; set; }
        public int BrokenProductsCount { get; set; }
        public int MissingProductsCount { get; set; }
        public int ImproperProductsCount { get; set; }
        public int ExcessProductsCount { get; set; }
        public int PurchaseOrdersCount { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? CompletedOn { get; set; }
        public DateTimeOffset? CancelledOn { get; set; }
        public string Reason { get; set; }
        public string DeliveryFormsUrl { get; set; }
        public Guid AssignedToId { get; set; }
        public virtual UserViewModel AssignedTo { get; set; }
    }
}