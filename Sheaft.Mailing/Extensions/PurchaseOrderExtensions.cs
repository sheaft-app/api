﻿using System.Linq;
using Newtonsoft.Json;
using Sheaft.Domain.Extensions;

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

            var address = purchaseOrder.ExpectedDelivery.Address;
            return new PurchaseOrderMailerModel 
            { 
                Id = purchaseOrderId,
                Lines = lines, 
                SenderName = senderName,
                Reference = purchaseOrder.Reference.AsPurchaseOrderIdentifier(), 
                VendorName = purchaseOrder.VendorInfo.Name,
                CreatedOn = purchaseOrder.CreatedOn,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                TotalOnSalePrice = purchaseOrder.TotalOnSalePrice, 
                TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice, 
                TotalVatPrice = purchaseOrder.TotalVatPrice,
                DeliveryFeesWholeSalePrice = purchaseOrder.ExpectedDelivery.DeliveryFeesWholeSalePrice,
                DeliveryFeesVatPrice = purchaseOrder.ExpectedDelivery.DeliveryFeesVatPrice,
                DeliveryFeesOnSalePrice = purchaseOrder.ExpectedDelivery.DeliveryFeesOnSalePrice,
                PurchaseOrderUrl = url,
                ExpectedFrom = purchaseOrder.ExpectedDelivery.From,
                ExpectedTo = purchaseOrder.ExpectedDelivery.To,
                Address = address != null ? $"{address.Line1} {address.Line2} {address.Zipcode} {address.City}" : null,
                Reason = purchaseOrder.Reason,
                ClientKind = purchaseOrder.SenderInfo.Kind
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
                ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate
            };
        }
    }
}