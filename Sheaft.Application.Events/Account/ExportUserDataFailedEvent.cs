using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class ExportUserDataFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-export-user-data-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public ExportUserDataFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
