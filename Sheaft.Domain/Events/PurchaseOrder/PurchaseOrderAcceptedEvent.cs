using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PurchaseOrder
{
    public class PurchaseOrderAcceptedEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderAcceptedEvent(Guid purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PurchaseOrderId { get; }
    }
}
