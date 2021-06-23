using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class DeliveryReceiptGeneratedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveryReceiptGeneratedEvent(Guid deliveryId)
        {
            DeliveryId = deliveryId;
        }

        public Guid DeliveryId { get; }
    }
}