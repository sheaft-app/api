using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class AvailablePurchaseOrderDto
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalWeight { get; set; }
        public int ProductsCount { get; set; }
        public int LinesCount { get; set; }
        public int ReturnablesCount { get; set; }
        public string Client { get; set; }
        public string Address { get; set; }
        public Guid DeliveryId { get; set; }
    }
}