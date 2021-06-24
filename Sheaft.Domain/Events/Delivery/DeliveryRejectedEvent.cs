using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class DeliveryRejectedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryRejectedEvent(Guid deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public Guid DeliveryId { get; }
    }
}