using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-payout-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
