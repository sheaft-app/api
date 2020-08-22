using System;
using Sheaft.Core;

namespace Sheaft.Application.Events
{
    public class AccountExportDataFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-accounts-export-failed";
        public const string MAILING_TEMPLATE_ID = "";

        public AccountExportDataFailedEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
