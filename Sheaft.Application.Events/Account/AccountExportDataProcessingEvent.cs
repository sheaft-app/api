using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class AccountExportDataProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-accounts-export-processing";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AccountExportDataProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
