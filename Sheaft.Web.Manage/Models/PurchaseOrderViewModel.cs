using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class PurchaseOrderViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? AcceptedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public DateTimeOffset? WithdrawnOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public string Reference { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public int LinesCount { get; set; }
        public int ProductsCount { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalWeight { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public UserProfileViewModel Sender { get; set; }
        public ExpectedPurchaseOrderDeliveryViewModel ExpectedDelivery { get; set; }
        public UserProfileViewModel Vendor { get; set; }
        public TransferInfoViewModel Transfer { get; set; }
        public IEnumerable<PurchaseOrderProductViewModel> Products { get; set; }

    }
}
