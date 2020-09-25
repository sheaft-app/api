using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DocumentOutdatedEvent : Event
    {
        public const string QUEUE_NAME = "event-document-outdated";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DocumentOutdatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
}
