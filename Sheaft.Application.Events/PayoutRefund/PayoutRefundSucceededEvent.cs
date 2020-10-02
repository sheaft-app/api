using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutRefundSucceededEvent : Event
    {
        [JsonConstructor]
        public PayoutRefundSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public string RefundIdentifier { get; set; }
    }
}
