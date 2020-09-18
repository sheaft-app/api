using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-payout-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
