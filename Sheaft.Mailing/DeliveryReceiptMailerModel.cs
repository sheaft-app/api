using System;
using System.Collections.Generic;

namespace Sheaft.Mailing
{
    public class DeliveryReceiptMailerModel
    {
        public DeliveryUserMailerModel Producer { get; set; }
        public DeliveryUserMailerModel Client { get; set; }
        public List<DeliveryPurchaseOrderMailerModel> PurchaseOrders { get; set; }
        public DateTimeOffset ScheduledOn { get; set; }
        public string ReceptionnedBy { get; set; }
        public string Comment { get; set; }
        public string Identifier { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
    }
}