using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Order
{
    public class OrderConfirmedEvent : DomainEvent
    {
        [JsonConstructor]
        public OrderConfirmedEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
