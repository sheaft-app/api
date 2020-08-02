using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{

    public class ExportPickingOrdersInput
    {
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public DateTimeOffset? ExpectedDeliveryDate { get; set; }
        public string Name { get; set; }
    }
}