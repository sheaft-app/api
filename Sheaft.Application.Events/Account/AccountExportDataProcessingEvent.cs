using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class AccountExportDataProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-accounts-export-processing";
        public const string MAILING_TEMPLATE_ID = "";

        public AccountExportDataProcessingEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
