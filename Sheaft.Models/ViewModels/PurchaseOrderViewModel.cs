using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
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
        public OrderStatusKind Status { get; set; }
        public UserViewModel Sender { get; set; }
        public ExpectedDeliveryViewModel ExpectedDelivery { get; set; }
        public UserViewModel Vendor { get; set; }
        public IEnumerable<PurchaseOrderProductViewModel> Products { get; set; }

    }
}
