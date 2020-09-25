using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutRefundSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-payout-refund-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutRefundSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RefundId { get; set; }
    }
}
