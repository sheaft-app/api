using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayinFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-payin-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayinFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
}
