using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayinSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-payin-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayinSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
