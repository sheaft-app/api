using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class CreatePurchaseOrderInput
    {
        public Guid ProducerId { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public Guid DeliveryModeId { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public string Comment { get; set; }
    }
}