using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public OrderStatusKind Status { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalReturnableOnSalePrice { get; set; }
        public decimal TotalReturnableWholeSalePrice { get; set; }
        public decimal TotalReturnableVatPrice { get; set; }
        public decimal TotalWeight { get; set; }
        public int ReturnablesCount { get; set; }
        public int LinesCount { get; set; }
        public int ProductsCount { get; set; }
        public decimal Donation { get; set; }
        public decimal Fees { get; set; }
        public virtual UserProfileDto User { get; set; }
    }
}