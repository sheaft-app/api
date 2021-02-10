using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PayinRefund
{
    public class CreateOrderPayinRefundFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public CreateOrderPayinRefundFailedEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
