using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class ShortPurchaseOrderViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? AcceptedOn { get; set; }
        public DateTimeOffset? CompletedOn { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
        public DateTimeOffset? WithdrawnOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public int Reference { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public int LinesCount { get; set; }
        public int ProductsCount { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalWeight { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public UserProfileViewModel SenderInfo { get; set; }
        public ExpectedPurchaseOrderDeliveryViewModel ExpectedDelivery { get; set; }
        public UserProfileViewModel VendorInfo { get; set; }
        public Guid ClientId { get; set; }
        public Guid ProducerId { get; set; }
    }
}
