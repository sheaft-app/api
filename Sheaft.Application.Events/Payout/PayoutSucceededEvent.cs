using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-payout-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
}
