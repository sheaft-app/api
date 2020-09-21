using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class ExportPickingOrdersInput
    {
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public DateTimeOffset? ExpectedDeliveryDate { get; set; }
        public string Name { get; set; }
    }
}