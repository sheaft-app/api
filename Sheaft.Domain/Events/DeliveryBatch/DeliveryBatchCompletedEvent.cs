using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.DeliveryBatch
{
    public class DeliveryBatchCompletedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryBatchCompletedEvent(Guid deliveryBatchId)
        {
            DeliveryBatchId = deliveryBatchId;
        }

        public Guid DeliveryBatchId { get; }
    }
}
