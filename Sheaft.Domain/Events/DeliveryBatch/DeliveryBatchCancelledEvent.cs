using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.DeliveryBatch
{
    public class DeliveryBatchCancelledEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryBatchCancelledEvent(Guid deliveryBatchId)
        {
            DeliveryBatchId = deliveryBatchId;
        }

        public Guid DeliveryBatchId { get; }
    }
}
