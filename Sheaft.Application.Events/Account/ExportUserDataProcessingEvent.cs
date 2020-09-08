using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ExportUserDataProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-export-user-data-processing";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public ExportUserDataProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
