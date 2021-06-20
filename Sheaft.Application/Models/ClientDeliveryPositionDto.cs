using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ClientDeliveryPositionDto
    {
        public Guid ClientId { get; set; }
        public int? Position { get; set; }
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }
}