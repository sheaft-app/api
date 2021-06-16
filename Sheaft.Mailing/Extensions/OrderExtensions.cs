using System.Linq;
using Newtonsoft.Json;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mailing.Extensions
{
    public static class OrderExtensions
    {
        public static OrderSummaryMailerModel GetTemplateData(this Domain.Order order, string orderId, string url)
        {
            var producers = order.Products
                .GroupBy(p => p.Producer)
                .Select(o => new ProducerMailerModel
                {
                    Delivery =
                        GetDelivery(order.Deliveries.FirstOrDefault(d => d.DeliveryMode.ProducerId == o.Key.Id)),
                    Name = o.Key.Name,
                    Address = GetAddress(order, order.Deliveries.FirstOrDefault(d => d.DeliveryMode.ProducerId == o.Key.Id)),
                    VatPrice = o.Sum(p => p.TotalVatPrice),
                    OnSalePrice = o.Sum(p => p.TotalOnSalePrice),
                    WholeSalePrice = o.Sum(p => p.TotalWholeSalePrice),
                    Products = o.ToList().Select(p => new ProductMailerModel
                    {
                        Name = p.Name,
                        Quantity = p.Quantity,
                        Reference = p.Reference,
                        VatPrice = p.TotalVatPrice,
                        WholeSalePrice = p.TotalWholeSalePrice,
                        OnSalePrice = p.TotalOnSalePrice
                    })
                });

            return new OrderSummaryMailerModel
            {
                CreatedOn = order.CreatedOn,
                Producers = producers,
                ProductsCount = order.Products.Count,
                OrderId = orderId,
                SenderKind = order.User.Kind,
                SenderName = order.User.Name,
                TotalPrice = order.TotalPrice,
                MyOrdersUrl = url,
                OnSalePrice = order.TotalOnSalePrice,
                WholeSalePrice = order.TotalWholeSalePrice,
                Donation = order.Donation,
                Fees = order.FeesPrice - order.DonationFeesPrice
            };
        }

        private static AddressMailerModel GetAddress(Order order, OrderDelivery delivery)
        {
            if (order.User.Kind == ProfileKind.Store)
                return new AddressMailerModel
                {
                    Line1 = order.User.Address.Line1,
                    Line2 = order.User.Address.Line2,
                    Zipcode = order.User.Address.Zipcode,
                    City = order.User.Address.City,
                };
        
            return new AddressMailerModel
            {
                Line1 = delivery.DeliveryMode.Address.Line1,
                Line2 = delivery.DeliveryMode.Address.Line2,
                Zipcode = delivery.DeliveryMode.Address.Zipcode,
                City = delivery.DeliveryMode.Address.City,
            };
        }

        private static ExpectedOrderDeliveryMailerModel GetDelivery(OrderDelivery orderDelivery)
        {
            return new ExpectedOrderDeliveryMailerModel
            {
                ExpectedDeliveryDate = orderDelivery.ExpectedDeliveryDate,
                Day = orderDelivery.Day,
                From = orderDelivery.From,
                To = orderDelivery.To
            };
        }

        public static string GetOrderNotifModelAsString(this Domain.Order order, string purchaseOrderId)
        {
            return JsonConvert.SerializeObject(GetOrderNotifModel(order, purchaseOrderId));
        }

        public static object GetOrderNotifModel(this Domain.Order order, string purchaseOrderId)
        {
            return new
            {
                PurchaseOrderId = purchaseOrderId,
                Status = order.Status,
                Reference = order.Reference,
                CreatedOn = order.CreatedOn,
                TotalPrice = order.TotalPrice,
                TotalOnSalePrice = order.TotalOnSalePrice,
                TotalWholeSalePrice = order.TotalWholeSalePrice
            };
        }
    }
}