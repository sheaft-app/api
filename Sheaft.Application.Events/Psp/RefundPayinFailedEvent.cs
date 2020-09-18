using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class RefundPayinFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-refund-payin-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public RefundPayinFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
