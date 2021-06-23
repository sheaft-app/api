using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class DeliveryFormGeneratedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryFormGeneratedEvent(Guid deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public Guid DeliveryId { get; }
    }
}
