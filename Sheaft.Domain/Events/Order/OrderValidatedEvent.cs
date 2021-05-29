using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Order
{
    public class OrderValidatedEvent : DomainEvent
    {
        [JsonConstructor]
        public OrderValidatedEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}