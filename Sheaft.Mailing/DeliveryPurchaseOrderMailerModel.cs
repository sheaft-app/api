using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class DeliveryPurchaseOrderMailerModel
    {
        public string Reference { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public List<DeliveryProductMailerModel> Products { get; set; }
        public List<DeliveryReturnableMailerModel> Returnables { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
    }
}