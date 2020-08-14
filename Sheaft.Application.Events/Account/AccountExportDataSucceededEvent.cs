using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class AccountExportDataSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-accounts-export-succeeded";
        public const string MAILING_TEMPLATE_ID = "d-279911b0285e406d8a4bb4e020d7d9b7";

        public AccountExportDataSucceededEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
