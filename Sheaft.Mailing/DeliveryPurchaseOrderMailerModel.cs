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
        public decimal TotalVat5Price { get; set; }
        public decimal TotalVat10Price { get; set; }
        public decimal TotalVat20Price { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal DeliveryFeesWholeSalePrice { get; set; }
        public decimal DeliveryFeesVatPrice { get; set; }
        public decimal DeliveryFeesOnSalePrice { get; set; }
    }
}