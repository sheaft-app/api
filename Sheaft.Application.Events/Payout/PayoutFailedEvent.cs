using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-payout-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
}
