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
        public static DeliveryFormMailerModel GetDeliveryFormModel(Domain.Producer producer, string producerSiret,
            Domain.User client, string clientSiret, Domain.Delivery delivery)
        {
            var purchaseOrders = GetPurchaseOrders(delivery.PurchaseOrders);

            var productsToDeliver = delivery.Products
                .Where(p => p.RowKind == ModificationKind.ToDeliver)
                .Select(p => GetProductModel(p))
                .ToList();

            var productsDiffs = delivery.Products
                .Where(p => p.RowKind != ModificationKind.ToDeliver)
                .Select(p => GetProductModel(p))
                .ToList();

            var returnablesDiffs = delivery.Products
                .Where(p => p.RowKind != ModificationKind.ToDeliver && p.HasReturnable)
                .GroupBy(p => new {p.ReturnableId, p.RowKind})
                .Select(p =>
                {
                    var returnable = p.First();
                    return new DeliveryReturnableMailerModel
                    {
                        Id = p.Key.ReturnableId.Value,
                        Name = returnable.ReturnableName,
                        Quantity = returnable.Quantity,
                        Vat = returnable.ReturnableVat.Value,
                        RowKind = p.Key.RowKind,
                        TotalVatPrice = p.Sum(r => r.TotalReturnableVatPrice.Value),
                        TotalOnSalePrice = p.Sum(r => r.TotalReturnableOnSalePrice.Value),
                        TotalWholeSalePrice = p.Sum(r => r.TotalReturnableWholeSalePrice.Value),
                        WholeSalePrice = returnable.ReturnableWholeSalePrice.Value
                    };
                }).ToList();

            var returnedReturnables = delivery.ReturnedReturnables
                .Select(r => GetReturnedReturnableModel(r))
                .ToList();

            return new DeliveryFormMailerModel()
            {
                Identifier = delivery.Reference.AsDeliveryIdentifier(),
                Producer = GetUserModel(producer, producerSiret),
                Client = GetUserModel(client, clientSiret),
                PurchaseOrders = purchaseOrders,
                ProductsToDeliver = productsToDeliver,
                ProductsDiffs = productsDiffs,
                ReturnedReturnables = returnedReturnables,
                ReturnablesDiffs = returnablesDiffs,
                DeliveredOn = delivery.DeliveredOn ?? delivery.ScheduledOn,
                ScheduledOn = delivery.ScheduledOn,
                ReceptionnedBy = delivery.ReceptionedBy,
                Comment = delivery.Comment,
                TotalWholeSalePrice = productsToDeliver.Sum(po => po.TotalWholeSalePrice) +
                                      productsDiffs.Sum(p => p.TotalWholeSalePrice) +
                                      returnedReturnables.Sum(p => p.TotalWholeSalePrice),
                TotalVatPrice = productsToDeliver.Sum(po => po.TotalVatPrice) +
                                productsDiffs.Sum(p => p.TotalVatPrice) + 
                                returnedReturnables.Sum(p => p.TotalVatPrice),
                TotalOnSalePrice = productsToDeliver.Sum(po => po.TotalOnSalePrice) +
                                   productsDiffs.Sum(p => p.TotalOnSalePrice) +
                                   returnedReturnables.Sum(p => p.TotalOnSalePrice),
            };
        }

        private static List<DeliveryPurchaseOrderMailerModel> GetPurchaseOrders(
            ICollection<Domain.PurchaseOrder> purchaseOrders)
        {
            var results = new List<DeliveryPurchaseOrderMailerModel>();
            foreach (var purchaseOrder in purchaseOrders)
            {
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
                    RowKind = ModificationKind.ToDeliver,
                    Vat = product.Vat,
                    ProductWholeSalePrice = product.UnitWholeSalePrice,
                    ProductTotalVatPrice = groupedProduct.Sum(po => po.TotalProductVatPrice),
                    ProductTotalWholeSalePrice = groupedProduct.Sum(po => po.TotalProductWholeSalePrice),
                    ProductTotalOnSalePrice = groupedProduct.Sum(po => po.TotalProductOnSalePrice),
                    HasReturnable = groupedProduct.FirstOrDefault()?.HasReturnable ?? false,
                    ReturnableName = groupedProduct.FirstOrDefault()?.Name,
                    ReturnableQuantity = product.Quantity,
                    ReturnableVat = groupedProduct.FirstOrDefault()?.Vat,
                    ReturnableWholeSalePrice = groupedProduct.FirstOrDefault()?.ReturnableWholeSalePrice,
                    ReturnableTotalVatPrice = groupedProduct.Sum(po => po.TotalReturnableVatPrice),
                    ReturnableTotalWholeSalePrice = groupedProduct.Sum(po => po.TotalReturnableWholeSalePrice),
                    ReturnableTotalOnSalePrice = groupedProduct.Sum(po => po.TotalReturnableOnSalePrice),
                    TotalVatPrice = groupedProduct.Sum(po => po.TotalVatPrice),
                    TotalWholeSalePrice = groupedProduct.Sum(po => po.TotalWholeSalePrice),
                    TotalOnSalePrice = groupedProduct.Sum(po => po.TotalOnSalePrice),
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
                ProductTotalVatPrice = p.TotalProductVatPrice,
                ProductTotalWholeSalePrice = p.TotalProductWholeSalePrice,
                ProductTotalOnSalePrice = p.TotalProductOnSalePrice,
                ProductWholeSalePrice = p.UnitWholeSalePrice,
                HasReturnable = p.HasReturnable,
                ReturnableName = p.ReturnableName,
                ReturnableQuantity = p.Quantity,
                ReturnableVat = p.ReturnableVat,
                ReturnableWholeSalePrice = p.ReturnableWholeSalePrice,
                ReturnableTotalVatPrice = p.TotalReturnableVatPrice,
                ReturnableTotalOnSalePrice = p.TotalReturnableOnSalePrice,
                ReturnableTotalWholeSalePrice = p.TotalReturnableWholeSalePrice,
                TotalVatPrice = p.TotalVatPrice,
                TotalOnSalePrice = p.TotalOnSalePrice,
                TotalWholeSalePrice = p.TotalWholeSalePrice
            };
        }

        private static List<DeliveryReturnableMailerModel> GetReturnablesModel(
            IEnumerable<IGrouping<Guid, ProductRow>> groupedProducts)
        {
            var returnables = new List<DeliveryReturnableMailerModel>();
            foreach (var groupedProduct in groupedProducts)
            {
                var product = groupedProduct.First();
                if (!product.HasReturnable)
                    continue;

                returnables.Add(new DeliveryReturnableMailerModel()
                {
                    Name = product.ReturnableName,
                    Quantity = product.Quantity,
                    RowKind = ModificationKind.ToDeliver,
                    Vat = product.ReturnableVat ?? 0,
                    TotalVatPrice = groupedProduct.Sum(po => po.TotalReturnableVatPrice ?? 0),
                    TotalWholeSalePrice = groupedProduct.Sum(po => po.TotalReturnableWholeSalePrice ?? 0),
                    TotalOnSalePrice = groupedProduct.Sum(po => po.TotalReturnableOnSalePrice ?? 0),
                    WholeSalePrice = product.ReturnableWholeSalePrice ?? 0
                });
            }

            return returnables;
        }

        private static DeliveryReturnableMailerModel GetReturnedReturnableModel(DeliveryReturnable p)
        {
            return new DeliveryReturnableMailerModel()
            {
                Name = p.Name,
                Quantity = p.Quantity,
                WholeSalePrice = p.UnitWholeSalePrice,
                Vat = p.Vat,
                TotalWholeSalePrice = p.TotalWholeSalePrice,
                TotalVatPrice = p.TotalVatPrice,
                TotalOnSalePrice = p.TotalOnSalePrice
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