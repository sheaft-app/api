using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PurchaseOrder
{
    public class PurchaseOrderProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderProcessingEvent(Guid purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PurchaseOrderId { get; }
    }
}
