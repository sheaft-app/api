using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ShortDeliveryViewModel
    {
        public Guid Id { get; set; }
        public int Reference { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Client { get; set; }
        public string Producer { get; set; }
        public DeliveryKind Kind { get; set; }
        public DeliveryStatus Status { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
        public DateTimeOffset? RejectedOn { get; set; }
        public int? Position { get; set; }
        public string ReceptionedBy { get; set; }
        public string Comment { get; set; }
        public AddressViewModel Address { get; set; }
        public int PurchaseOrdersCount { get; set; }
        public int ProductsToDeliverCount { get; set; }
        public int ProductsDeliveredCount { get; set; }
        public int ReturnablesCount { get; set; }
        public int BrokenProductsCount { get; set; }
        public int ImproperProductsCount { get; set; }
        public int MissingProductsCount { get; set; }
        public int ExcessProductsCount { get; set; }
        public int ReturnedReturnablesCount { get; set; }
        public string DeliveryFormUrl { get; set; }
        public string DeliveryReceiptUrl { get; set; }
        public Guid? DeliveryBatchId { get; set; }
        public Guid ProducerId { get; set; }
        public Guid ClientId { get; set; }
    }
}