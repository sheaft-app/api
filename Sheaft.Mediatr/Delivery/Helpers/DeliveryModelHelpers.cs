using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Extensions;
using Sheaft.Mailing;

namespace Sheaft.Mediatr.Delivery.Commands
{
    internal static class DeliveryModelHelpers
    {
        public static DeliveryFormMailerModel GetDeliveryFormModel(Domain.Producer producer, string producerSiret, Domain.User client, string clientSiret, Domain.Delivery delivery)
        {
            var purchaseOrders = GetPurchaseOrders(delivery.PurchaseOrders);
            var returnedProducts = delivery.Products
                .Where(p => p.RowKind != ModificationKind.Deliver)
                .Select(p => GetProductModel(p))
                .ToList();
            var returnedReturnables = delivery.ReturnedReturnables
                .Select(r => GetReturnedReturnableModel(r))
                .ToList();
            
            return new DeliveryFormMailerModel()
            {
                Identifier = delivery.Reference.AsDeliveryIdentifier(),
                Producer = GetUserModel(producer, producerSiret),
                Client = GetUserModel(client, clientSiret),
                PurchaseOrders = purchaseOrders,
                Products = delivery.Products.Select(p => GetProductModel(p)).ToList(),
                ReturnedProducts = returnedProducts,
                ReturnedReturnables = returnedReturnables,
                DeliveredOn = delivery.DeliveredOn ?? delivery.ScheduledOn,
                ScheduledOn = delivery.ScheduledOn,
                ReceptionnedBy = delivery.ReceptionedBy,
                Comment = delivery.Comment,
                TotalWholeSalePrice = purchaseOrders.Sum(po => po.TotalWholeSalePrice) + returnedProducts.Sum(p => p.TotalWholeSalePrice) + returnedReturnables.Sum(p => p.TotalWholeSalePrice),
                TotalVatPrice = purchaseOrders.Sum(po => po.TotalVatPrice) + returnedProducts.Sum(p => p.TotalVatPrice) + returnedReturnables.Sum(p => p.TotalVatPrice),
                TotalOnSalePrice = purchaseOrders.Sum(po => po.TotalOnSalePrice) + returnedProducts.Sum(p => p.TotalOnSalePrice) + returnedReturnables.Sum(p => p.TotalOnSalePrice),
            };
        }

        private static List<DeliveryPurchaseOrderMailerModel> GetPurchaseOrders(
            ICollection<Domain.PurchaseOrder> purchaseOrders)
        {
            var results = new List<DeliveryPurchaseOrderMailerModel>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                var purchaseOrderProducts = purchaseOrder.Products
                    .Where(p => p.RowKind == ModificationKind.Deliver)
                    .ToList();

                var products = purchaseOrder.Products.GroupBy(p => p.ProductId);
                results.Add(new DeliveryPurchaseOrderMailerModel
                {
                    Reference = purchaseOrder.Reference.AsPurchaseOrderIdentifier(),
                    CreatedOn = purchaseOrder.CreatedOn,
                    Products = GetProductsModel(products),
                    Returnables = GetReturnablesModel(products),
                    TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice,
                    TotalVatPrice = purchaseOrder.TotalVatPrice,
                    TotalOnSalePrice = purchaseOrder.TotalOnSalePrice,
                });
            }

            return results;
        }

        private static List<DeliveryProductMailerModel> GetProductsModel(
            IEnumerable<IGrouping<Guid, ProductRow>> groupedProducts)
        {
            var products = new List<DeliveryProductMailerModel>();
            foreach (var groupedProduct in groupedProducts)
            {
                var product = groupedProduct.First();
                products.Add(new DeliveryProductMailerModel()
                {
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Reference = product.Reference,
                    RowKind = ModificationKind.Deliver,
                    Vat = product.Vat,
                    TotalVatPrice = groupedProduct.Sum(po => po.TotalVatPrice),
                    TotalWholeSalePrice = groupedProduct.Sum(po => po.TotalWholeSalePrice),
                    TotalOnSalePrice = groupedProduct.Sum(po => po.TotalOnSalePrice),
                    UnitWholeSalePrice = product.UnitWholeSalePrice
                });
            }

            return products;
        }

        private static DeliveryProductMailerModel GetProductModel(DeliveryProduct p)
        {
            return new DeliveryProductMailerModel()
            {
                Name = p.Name,
                Quantity = p.Quantity,
                Reference = p.Reference,
                RowKind = p.RowKind,
                Vat = p.Vat,
                TotalVatPrice = p.TotalVatPrice,
                TotalWholeSalePrice = p.TotalWholeSalePrice,
                TotalOnSalePrice = p.TotalOnSalePrice,
                UnitWholeSalePrice = p.UnitWholeSalePrice
            };
        }

        private static List<DeliveryReturnableMailerModel> GetReturnablesModel(
            IEnumerable<IGrouping<Guid, ProductRow>> groupedProducts)
        {
            var products = new List<DeliveryReturnableMailerModel>();
            foreach (var groupedProduct in groupedProducts)
            {
                var product = groupedProduct.First();
                products.Add(new DeliveryReturnableMailerModel()
                {
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Vat = product.Vat,
                    TotalVatPrice = groupedProduct.Sum(po => po.TotalVatPrice),
                    TotalWholeSalePrice = groupedProduct.Sum(po => po.TotalWholeSalePrice),
                    TotalOnSalePrice = groupedProduct.Sum(po => po.TotalOnSalePrice),
                    UnitWholeSalePrice = product.UnitWholeSalePrice
                });
            }

            return products;
        }

        private static DeliveryReturnableMailerModel GetReturnedReturnableModel(DeliveryReturnable p)
        {
            return new DeliveryReturnableMailerModel()
            {
                Name = p.Name,
                Quantity = p.Quantity,
                UnitWholeSalePrice = p.UnitWholeSalePrice,
                Vat = p.Vat,
                TotalWholeSalePrice = p.TotalWholeSalePrice,
            };
        }

        private static DeliveryUserMailerModel GetUserModel(Domain.User client, string siret)
        {
            return new DeliveryUserMailerModel
            {
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Siret = siret,
                Address = new DeliveryAddressMailerModel()
                {
                    Line1 = client.Address.Line1,
                    Line2 = client.Address.Line2,
                    Zipcode = client.Address.Zipcode,
                    City = client.Address.City,
                }
            };
        }
    }
}