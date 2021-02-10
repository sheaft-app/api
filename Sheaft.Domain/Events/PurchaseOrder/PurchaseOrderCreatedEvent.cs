using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PurchaseOrder
{
    public class PurchaseOrderCreatedEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderCreatedEvent(Guid purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PurchaseOrderId { get; }
    }
}
