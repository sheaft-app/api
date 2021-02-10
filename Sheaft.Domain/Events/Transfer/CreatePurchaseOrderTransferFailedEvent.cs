using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transfer
{
    public class CreatePurchaseOrderTransferFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public CreatePurchaseOrderTransferFailedEvent(Guid purchaseOrderId)
        {
            PurchaseOrderId = purchaseOrderId;
        }

        public Guid PurchaseOrderId { get; }
    }
}
