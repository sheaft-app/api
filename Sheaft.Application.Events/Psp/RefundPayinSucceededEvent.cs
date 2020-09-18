using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class RefundPayinSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-refund-payin-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public RefundPayinSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
