using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Events
{
    public class AgreementCancelledEvent : Event
    {
        public const string QUEUE_NAME = "event-agreement-cancelled";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementCancelledEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
