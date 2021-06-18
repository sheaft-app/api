using System;

namespace Sheaft.Application.Models
{
    public class PurchaseOrderDeliveryPositionDto
    {
        public Guid DeliveryId { get; set; }
        public int? Position { get; set; }
    }
}