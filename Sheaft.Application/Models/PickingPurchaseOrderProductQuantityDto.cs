using System;

namespace Sheaft.Application.Models
{
    public class PickingPurchaseOrderProductQuantityDto
    {
        public Guid PurchaseOrderId { get; set; }
        public int PreparedQuantity { get; set; }
    }
}