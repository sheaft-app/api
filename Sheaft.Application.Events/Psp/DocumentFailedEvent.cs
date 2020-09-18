using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DocumentFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-document-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DocumentFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
}
