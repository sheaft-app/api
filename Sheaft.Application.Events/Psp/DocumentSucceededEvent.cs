using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DocumentSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-document-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DocumentSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
}
