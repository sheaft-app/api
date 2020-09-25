using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class ExportUserDataFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-user-export-data-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public ExportUserDataFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
