using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Dto
{
    public class OrderDto
    {
        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal Donation { get; set; }
        public decimal Fees { get; set; }
        public virtual UserDto User { get; set; }
    }
}