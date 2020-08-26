using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class AccountExportDataFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-accounts-export-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public AccountExportDataFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
