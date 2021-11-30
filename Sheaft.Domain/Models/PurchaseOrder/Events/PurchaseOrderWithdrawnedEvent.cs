using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PurchaseOrder
{
    public class PurchaseOrderWithdrawnedEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderWithdrawnedEvent(Guid purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PurchaseOrderId { get; }
    }
}