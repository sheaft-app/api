using System.Linq;
using Newtonsoft.Json;
using Sheaft.Application.Models.Mailer;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Common.Extensions
{
    public static class PurchaseOrderExtensions
    {
        public static PurchaseOrderMailerModel GetTemplateDatas(this PurchaseOrder purchaseOrder, string url)
        {
            var senderName = purchaseOrder.Sender.Name;
            var lines = purchaseOrder.Products.Select(o => new PurchaseOrderLineMailerModel 
            { 
                Line_Name = o.Name, 
                Line_Quantity = o.Quantity, 
                Line_Price = o.TotalOnSalePrice 
            }).ToList();

            var address = purchaseOrder.ExpectedDelivery.Address;
            return new PurchaseOrderMailerModel 
            { 
                Lines = lines, 
                SenderName = senderName,
                Reference = purchaseOrder.Reference, 
                VendorName = purchaseOrder.Vendor.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                TotalOnSalePrice = purchaseOrder.TotalOnSalePrice, 
                PurchaseOrderUrl = url,
                VatPrice = purchaseOrder.TotalVatPrice,
                ExpectedFrom = purchaseOrder.ExpectedDelivery.From,
                ExpectedTo = purchaseOrder.ExpectedDelivery.To,
                Address = address != null ? $"{address.Line1} {address.Line2} {address.Zipcode} {address.City}" : null,
                Reason = purchaseOrder.Reason
            };
        }

        public static string GetPurchaseNotifModelAsString(this PurchaseOrder purchaseOrder)
        {
            return JsonConvert.SerializeObject(GetPurchaseNotifModel(purchaseOrder));
        }

        public static object GetPurchaseNotifModel(this PurchaseOrder purchaseOrder)
        {
            return new
            {
                PurchaseOrderId = purchaseOrder.Id,
                Status = purchaseOrder.Status,
                Reference = purchaseOrder.Reference,
                VendorName = purchaseOrder.Vendor.Name,
                SenderName = purchaseOrder.Sender.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate
            };
        }
    }
}