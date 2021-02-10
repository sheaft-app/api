using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models.Mailer
{
    public class PurchaseOrderMailerModel
    {
        public string SenderName { get; set; }
        public string Reference { get; set; }
        public string VendorName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public string PurchaseOrderUrl { get; set; }
        public IEnumerable<PurchaseOrderLineMailerModel> Lines { get; set; }
        public decimal VatPrice { get; set; }
        public TimeSpan ExpectedFrom { get; set; }
        public TimeSpan ExpectedTo { get; set; }
        public string Address { get; set; }
        public string Reason { get; set; }
    }
}
