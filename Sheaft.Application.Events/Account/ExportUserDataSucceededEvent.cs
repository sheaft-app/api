using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ExportUserDataSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-export-user-data-succeeded";
        public const string MAILING_TEMPLATE_ID = "d-279911b0285e406d8a4bb4e020d7d9b7";

        [JsonConstructor]
        public ExportUserDataSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
