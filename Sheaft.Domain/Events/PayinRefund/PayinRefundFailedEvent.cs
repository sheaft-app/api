using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PayinRefund
{
    public class PayinRefundFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PayinRefundFailedEvent(Guid refundId)
        {
            RefundId = refundId;
        }

        public Guid RefundId { get; }
    }
}
