using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Order
{
    public class OrderRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public OrderRefusedEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}