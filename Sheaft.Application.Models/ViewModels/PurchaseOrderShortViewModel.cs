using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class PurchaseOrderShortViewModel
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public PurchaseOrderStatus Status { get; set; }
    }
}
