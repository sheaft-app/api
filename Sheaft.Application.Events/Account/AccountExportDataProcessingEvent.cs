using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class AccountExportDataProcessingEvent : Event
    {
        public const string QUEUE_NAME = "accountexportdataprocessing";
        public const string MAILING_TEMPLATE_ID = "";

        public AccountExportDataProcessingEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
