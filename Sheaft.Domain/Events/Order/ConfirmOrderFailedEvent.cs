using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Order
{
    public class ConfirmOrderFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public ConfirmOrderFailedEvent(Guid orderId, string message)
        {
            OrderId = orderId;
            Message = message;
        }

        public Guid OrderId { get; }
        public string Message { get; }
    }
}
