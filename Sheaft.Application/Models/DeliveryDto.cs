using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public DeliveryKind Kind { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<DeliveryHourDto> DeliveryHours { get; set; }
        public IEnumerable<ClosingDto> Closings { get; set; }
        public decimal? DeliveryFeesWholeSalePrice { get; set; }
        public decimal? DeliveryFeesVatPrice { get; set; }
        public decimal? DeliveryFeesOnSalePrice { get; set; }
        public decimal? DeliveryFeesMinPurchaseOrdersAmount { get; set; }
        public DeliveryFeesApplication? ApplyDeliveryFeesWhen { get; set; }
        public decimal? AcceptPurchaseOrdersWithAmountGreaterThan { get; set; }
    }
}