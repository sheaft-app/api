using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutRefundFailedEvent : Event
    {
        [JsonConstructor]
        public PayoutRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public string RefundIdentifier { get; set; }
    }
}
