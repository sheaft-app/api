using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class DeliveryCompletedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryCompletedEvent(Guid deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public Guid DeliveryId { get; }
    }
}
