using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Mailing
{
    public class PurchaseOrderMailerModel
    {
        public string SenderName { get; set; }
        public string Reference { get; set; }
        public string VendorName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal DeliveryFeesWholeSalePrice { get; set; }
        public decimal DeliveryFeesVatPrice { get; set; }
        public decimal DeliveryFeesOnSalePrice { get; set; }
        public string PurchaseOrderUrl { get; set; }
        public IEnumerable<PurchaseOrderLineMailerModel> Lines { get; set; }
        public decimal TotalVatPrice { get; set; }
        public TimeSpan ExpectedFrom { get; set; }
        public TimeSpan ExpectedTo { get; set; }
        public string Address { get; set; }
        public string Reason { get; set; }
        public string Id { get; set; }
        public ProfileKind ClientKind { get; set; }
    }
}
