using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Extensions;

namespace Sheaft.Mailing.Helpers
{
    public static class DeliveryModelHelpers
    {
        public static DeliveryFormMailerModel GetDeliveryFormModel(Domain.User producer, Domain.User client,
            Domain.Delivery delivery, string producerSiret = null, string clientSiret = null)
        {
            var purchaseOrders = GetPurchaseOrders(delivery.PurchaseOrders);

            var productsToDeliver = delivery.Products
                .Where(p => p.RowKind == ModificationKind.ToDeliver)
                .Select(p => GetProductModel(p, p.RowKind))
                .ToList();

            var productsDiffs = delivery.Products
                .Where(p => p.RowKind != ModificationKind.ToDeliver)
                .Select(p => GetProductModel(p, p.RowKind))
                .ToList();

            var returnablesDiffs = delivery.Products
                .Where(p => p.RowKind != ModificationKind.ToDeliver && p.HasReturnable)
                .GroupBy(p => new { p.ReturnableId, p.RowKind })
                .Select(p =>
                {
                    var returnable = p.First();
                    return new DeliveryReturnableMailerModel
                    {
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

            var returnedReturnables = delivery.ReturnedReturnables?
                .Select(GetReturnedReturnableModel)
                .ToList() ?? new List<DeliveryReturnableMailerModel>();

            var vat5Price = GetVatPriceForPercent(productsToDeliver, productsDiffs, returnedReturnables, 5.5m);
            var vat10Price = GetVatPriceForPercent(productsToDeliver, productsDiffs, returnedReturnables, 10m);
            var vat20Price = GetVatPriceForPercent(productsToDeliver, productsDiffs, returnedReturnables, 20m) + delivery.DeliveryFeesVatPrice;
            
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
                DeliveryFeesWholeSalePrice = delivery.DeliveryFeesWholeSalePrice,
                DeliveryFeesVatPrice = delivery.DeliveryFeesVatPrice,
                DeliveryFeesOnSalePrice = delivery.DeliveryFeesOnSalePrice,
                TotalWholeSalePrice = productsToDeliver.Sum(po => po.TotalWholeSalePrice) +
                                      productsDiffs.Sum(p => p.TotalWholeSalePrice) +
                                      returnedReturnables.Sum(p => p.TotalWholeSalePrice) +
                                      delivery.DeliveryFeesWholeSalePrice,
                TotalVat5Price = vat5Price,
                TotalVat10Price = vat10Price,
                TotalVat20Price = vat20Price,
                TotalVatPrice = vat5Price + vat10Price + vat20Price,
                TotalOnSalePrice = productsToDeliver.Sum(po => po.TotalOnSalePrice) +
                                   productsDiffs.Sum(p => p.TotalOnSalePrice) +
                                   returnedReturnables.Sum(p => p.TotalOnSalePrice) +
                                   delivery.DeliveryFeesOnSalePrice,
            };
        }

        private static decimal GetVatPriceForPercent(List<DeliveryProductMailerModel> productsToDeliver, List<DeliveryProductMailerModel> productsDiffs, List<DeliveryReturnableMailerModel> returnedReturnables, decimal percent)
        {
            return productsToDeliver.Where(p => p.Vat == percent).Sum(p => p.ProductTotalVatPrice)
                + productsToDeliver.Where(p => p.ReturnableVat == percent).Sum(p => p.ReturnableTotalVatPrice ?? 0)
                + productsDiffs.Where(p => p.Vat == percent).Sum(p => p.ProductTotalVatPrice)
                + productsDiffs.Where(p => p.ReturnableVat == percent).Sum(p => p.ReturnableTotalVatPrice ?? 0)
                + returnedReturnables.Where(p => p.Vat == percent).Sum(p => p.TotalVatPrice);
        }

        private static List<DeliveryPurchaseOrderMailerModel> GetPurchaseOrders(
            ICollection<Domain.PurchaseOrder> purchaseOrders)
        {
            var results = new List<DeliveryPurchaseOrderMailerModel>();
            foreach (var purchaseOrder in purchaseOrders)
            {
                var po = new DeliveryPurchaseOrderMailerModel
                {
                    Reference = purchaseOrder.Reference.AsPurchaseOrderIdentifier(),
                    CreatedOn = purchaseOrder.CreatedOn,
                    ExpectedDeliveryDate = purchaseOrder.ExpectedDelivery.ExpectedDeliveryDate,
                    Products = GetProductsModel(purchaseOrder),
                    Returnables = GetReturnablesModel(purchaseOrder),
                    DeliveryFeesWholeSalePrice = purchaseOrder.ExpectedDelivery.DeliveryFeesWholeSalePrice,
                    DeliveryFeesVatPrice = purchaseOrder.ExpectedDelivery.DeliveryFeesVatPrice,
                    DeliveryFeesOnSalePrice = purchaseOrder.ExpectedDelivery.DeliveryFeesOnSalePrice,
                };

                if (purchaseOrder.PickingId.HasValue)
                {
                    po.TotalVat5Price =
                        po.Products.Where(p => p.Vat == 5.5m).Sum(p => p.ProductTotalVatPrice)
                        + po.Products.Where(p => p.ReturnableVat == 5.5m).Sum(p => p.ReturnableTotalVatPrice) ?? 0;
                    po.TotalVat10Price =
                        po.Products.Where(p => p.Vat == 10m).Sum(p => p.ProductTotalVatPrice)
                        + po.Products.Where(p => p.ReturnableVat == 10m).Sum(p => p.ReturnableTotalVatPrice) ?? 0;
                    po.TotalVat20Price =
                        po.Products.Where(p => p.Vat == 20m).Sum(p => p.ProductTotalVatPrice)
                        + po.Products.Where(p => p.ReturnableVat == 20m).Sum(p => p.ReturnableTotalVatPrice) ?? 0
                        + po.DeliveryFeesVatPrice;
                    po.TotalOnSalePrice = po.Products.Sum(p => p.TotalOnSalePrice) + po.DeliveryFeesOnSalePrice;
                    po.TotalWholeSalePrice =
                        po.Products.Sum(p => p.TotalWholeSalePrice) + po.DeliveryFeesWholeSalePrice;
                }
                else
                {
                    po.TotalVat5Price =
                        po.Products.Where(p => p.Vat == 5.5m).Sum(p => p.ProductTotalVatPrice)
                        + po.Products.Where(p => p.ReturnableVat == 5.5m).Sum(p => p.ReturnableTotalVatPrice) ?? 0;
                    po.TotalVat10Price =
                        po.Products.Where(p => p.Vat == 10m).Sum(p => p.ProductTotalVatPrice)
                        + po.Products.Where(p => p.ReturnableVat == 10m).Sum(p => p.ReturnableTotalVatPrice) ?? 0;
                    po.TotalVat20Price =
                        po.Products.Where(p => p.Vat == 20m).Sum(p => p.ProductTotalVatPrice)
                        + po.Products.Where(p => p.ReturnableVat == 20m).Sum(p => p.ReturnableTotalVatPrice) ?? 0
                        + po.DeliveryFeesVatPrice;
                    po.TotalOnSalePrice = purchaseOrder.TotalOnSalePrice + po.DeliveryFeesOnSalePrice;
                    po.TotalWholeSalePrice = purchaseOrder.TotalWholeSalePrice + po.DeliveryFeesWholeSalePrice;
                }

                results.Add(po);
            }

            return results;
        }

        private static List<DeliveryProductMailerModel> GetProductsModel(Domain.PurchaseOrder purchaseOrder)
        {
            var products = new List<DeliveryProductMailerModel>();
            if (purchaseOrder.PickingId.HasValue)
            {
                products = purchaseOrder.Picking.PreparedProducts
                    .Where(p => p.PurchaseOrderId == purchaseOrder.Id)
                    .Select(p => GetProductModel(p, ModificationKind.ToDeliver))
                    .ToList();
            }
            else
            {
                products = purchaseOrder.Products
                    .Select(p => GetProductModel(p, ModificationKind.ToDeliver))
                    .ToList();
            }

            return products;
        }

        private static DeliveryProductMailerModel GetProductModel(PreparedProduct p, ModificationKind rowKind)
        {
            var model = GetProductModel((ProductRow)p, rowKind);
            model.Batches = p.Batches?.Select(b => new BatchMailerModel
            {
                Number = b.Batch.Number,
                DLC = b.Batch.DLC,
                DDM = b.Batch.DDM
            }).ToList() ?? new List<BatchMailerModel>();
            return model;
        }

        private static DeliveryProductMailerModel GetProductModel(ProductRow p, ModificationKind rowKind)
        {
            return new DeliveryProductMailerModel()
            {
                Name = p.Name,
                Quantity = p.Quantity,
                Reference = p.Reference,
                Conditioning = p.GetConditioning(),
                RowKind = rowKind,
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

        private static List<DeliveryReturnableMailerModel> GetReturnablesModel(Domain.PurchaseOrder purchaseOrder)
        {
            var returnables = new List<DeliveryReturnableMailerModel>();
            if (purchaseOrder.PickingId.HasValue)
            {
                returnables = purchaseOrder.Picking.PreparedProducts
                    .Where(p => p.HasReturnable && p.PurchaseOrderId == purchaseOrder.Id && p.Quantity > 0)
                    .Select(p => GetReturnableModel(p, ModificationKind.ToDeliver))
                    .ToList();
            }
            else
            {
                returnables = purchaseOrder.Products
                    .Where(p => p.HasReturnable)
                    .Select(p => GetReturnableModel(p, ModificationKind.ToDeliver))
                    .ToList();
            }

            return returnables;
        }

        private static DeliveryReturnableMailerModel GetReturnableModel(ProductRow product, ModificationKind rowKind)
        {
            return new DeliveryReturnableMailerModel()
            {
                Name = product.ReturnableName,
                Quantity = product.Quantity,
                RowKind = rowKind,
                Vat = product.ReturnableVat ?? 0,
                TotalVatPrice = product.TotalReturnableVatPrice ?? 0,
                TotalWholeSalePrice = product.TotalReturnableWholeSalePrice ?? 0,
                TotalOnSalePrice = product.TotalReturnableOnSalePrice ?? 0,
                WholeSalePrice = product.ReturnableWholeSalePrice ?? 0
            };
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