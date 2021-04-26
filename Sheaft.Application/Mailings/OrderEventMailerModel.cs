using System;
using System.Collections.Generic;
using Sheaft.Application.Models;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Mailings
{
    public class OrderEventMailerModel
    {
        public string SenderName { get; set; }
        public ProfileKind SenderKind { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal OnSalePrice { get; set; }
        public int ProductsCount { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid OrderId { get; set; }
        public string MyOrdersUrl { get; set; }
        public decimal Donation { get; set; }
        public decimal Fees { get; set; }
        public IEnumerable<ProducerMailerModel> Producers { get; set; }
    }

    public class ProducerMailerModel
    {
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public ExpectedOrderDeliveryDto Delivery { get; set; }
        public IEnumerable<ProductMailerModel> Products { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal VatPrice { get; set; }
    }

    public class ProductMailerModel
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal OnSalePrice { get; set; }
        public decimal VatPrice { get; set; }
        public int Quantity { get; set; }
    }
}