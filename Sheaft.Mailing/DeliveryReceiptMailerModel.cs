using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class DeliveryReceiptMailerModel
    {
        public DeliveryUserMailerModel Producer { get; set; }
        public DeliveryUserMailerModel Client { get; set; }
        public List<DeliveryPurchaseOrderMailerModel> PurchaseOrders { get; set; }
        public List<DeliveryProductMailerModel> ProductsToDeliver { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public string ReceptionnedBy { get; set; }
        public string Comment { get; set; }
        public string Identifier { get; set; }
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