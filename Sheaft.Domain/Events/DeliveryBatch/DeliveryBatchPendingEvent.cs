using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.DeliveryBatch
{
    public class DeliveryBatchPendingEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryBatchPendingEvent(Guid deliveryBatchId)
        {
            DeliveryBatchId = deliveryBatchId;
        }

        public Guid DeliveryBatchId { get; }
    }
}
