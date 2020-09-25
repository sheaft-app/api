using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedEvent : Event
    {
        public const string QUEUE_NAME = "event-agreement-refused";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AgreementRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
