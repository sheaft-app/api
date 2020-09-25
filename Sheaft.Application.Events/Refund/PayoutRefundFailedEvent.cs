using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutRefundFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-payout-refund-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RefundId { get; set; }
    }
}
