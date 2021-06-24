using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.DeliveryBatch
{
    public class DeliveryBatchPostponedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryBatchPostponedEvent(Guid deliveryBatchId)
        {
            DeliveryBatchId = deliveryBatchId;
        }

        public Guid DeliveryBatchId { get; }
    }
}
