using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class AvailableClientDeliveryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int PurchaseOrdersCount { get; set; }
        public List<AvailablePurchaseOrderDto> PurchaseOrders { get; set; }
    }
}