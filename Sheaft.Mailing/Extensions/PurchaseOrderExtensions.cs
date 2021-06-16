using System.Linq;
using Newtonsoft.Json;

namespace Sheaft.Mailing.Extensions
{
    public static class PurchaseOrderExtensions
    {
        public static PurchaseOrderMailerModel GetTemplateData(this Domain.PurchaseOrder purchaseOrder, string purchaseOrderId, string url)
        {
            var senderName = purchaseOrder.SenderInfo.Name;
            var lines = purchaseOrder.Products.Select(o => new PurchaseOrderLineMailerModel 
            { 
                Line_Name = o.Name, 
                Line_Quantity = o.Quantity, 
                Line_Price = o.TotalOnSalePrice 
            }).ToList();

            var address = purchaseOrder.Delivery.Address;
            return new PurchaseOrderMailerModel 
            { 
                Id = purchaseOrderId,
                Lines = lines, 
                SenderName = senderName,
                Reference = purchaseOrder.Reference, 
                VendorName = purchaseOrder.VendorInfo.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                ExpectedDeliveryDate = purchaseOrder.Delivery.ExpectedDeliveryDate,
                TotalOnSalePrice = purchaseOrder.TotalOnSalePrice, 
                TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice, 
                PurchaseOrderUrl = url,
                VatPrice = purchaseOrder.TotalVatPrice,
                ExpectedFrom = purchaseOrder.Delivery.From,
                ExpectedTo = purchaseOrder.Delivery.To,
                Address = address != null ? $"{address.Line1} {address.Line2} {address.Zipcode} {address.City}" : null,
                Reason = purchaseOrder.Reason
            };
        }

        public static string GetPurchaseNotifModelAsString(this Domain.PurchaseOrder purchaseOrder, string purchaseOrderId)
        {
            return JsonConvert.SerializeObject(GetPurchaseNotifModel(purchaseOrder, purchaseOrderId));
        }

        public static object GetPurchaseNotifModel(this Domain.PurchaseOrder purchaseOrder, string purchaseOrderId)
        {
            return new
            {
                PurchaseOrderId = purchaseOrderId,
                Status = purchaseOrder.Status,
                Reference = purchaseOrder.Reference,
                VendorName = purchaseOrder.VendorInfo.Name,
                SenderName = purchaseOrder.SenderInfo.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                TotalOnSalePrice = purchaseOrder.TotalOnSalePrice,
                TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice,
                ExpectedDeliveryDate = purchaseOrder.Delivery.ExpectedDeliveryDate
            };
        }
    }
}