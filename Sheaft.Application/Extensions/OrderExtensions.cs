using System.Linq;
using Newtonsoft.Json;
using Sheaft.Application.Mailings;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Extensions
{
    public static class OrderExtensions
    {
        public static OrderEventMailerModel GetTemplateData(this Domain.Order order, string url)
        {
            var producers = order.Products
                .GroupBy(p => p.Producer)
                .Select(o => new ProducerMailerModel
                {
                    Delivery =
                        GetDelivery(order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == o.Key.Id)),
                    Name = o.Key.Name,
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

            return new OrderEventMailerModel
            {
                CreatedOn = order.CreatedOn,
                Producers = producers,
                ProductsCount = order.Products.Count,
                OrderId = order.Id,
                SenderKind = order.User.Kind,
                SenderName = order.User.Name,
                TotalPrice = order.TotalPrice,
                MyOrdersUrl = url,
                OnSalePrice = order.TotalOnSalePrice,
                WholeSalePrice = order.TotalWholeSalePrice
            };
        }

        private static ExpectedOrderDeliveryDto GetDelivery(OrderDelivery orderDelivery)
        {
            return new ExpectedOrderDeliveryDto
            {
                Day = orderDelivery.ExpectedDelivery.ExpectedDeliveryDate.DayOfWeek,
                From = orderDelivery.ExpectedDelivery.From,
                To = orderDelivery.ExpectedDelivery.To
            };
        }

        public static string GetOrderNotifModelAsString(this Domain.Order order)
        {
            return JsonConvert.SerializeObject(GetOrderNotifModel(order));
        }

        public static object GetOrderNotifModel(this Domain.Order order)
        {
            return new
            {
                PurchaseOrderId = order.Id,
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