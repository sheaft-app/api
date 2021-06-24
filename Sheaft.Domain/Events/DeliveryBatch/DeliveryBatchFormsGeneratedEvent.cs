using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.DeliveryBatch
{
    public class DeliveryBatchFormsGeneratedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryBatchFormsGeneratedEvent(Guid deliveryBatchId)
        {
            DeliveryBatchId = deliveryBatchId;
        }

        public Guid DeliveryBatchId { get; }
    }
}