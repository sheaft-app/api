using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public DateTimeOffset? AcceptedOn { get; set; }
        public DateTimeOffset? CompletedOn { get; set; }
        public DateTimeOffset? DeliveredOn { get; set; }
        public DateTimeOffset? WithdrawnOn { get; set; }
        public string Reference { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public int LinesCount { get; set; }
        public int ProductsCount { get; set; }
        public int ReturnablesCount { get; set; }
        public decimal TotalProductWholeSalePrice { get; set; }
        public decimal TotalProductVatPrice { get; set; }
        public decimal TotalProductOnSalePrice { get; set; }
        public decimal TotalReturnableOnSalePrice { get; set; }
        public decimal TotalReturnableWholeSalePrice { get; set; }
        public decimal TotalReturnableVatPrice { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalWeight { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public UserProfileDto Sender { get; set; }
        public ExpectedDeliveryDto ExpectedDelivery { get; set; }
        public UserProfileDto Vendor { get; set; }
        public IEnumerable<PurchaseOrderProductQuantityDto> Products { get; set; }
    }
}