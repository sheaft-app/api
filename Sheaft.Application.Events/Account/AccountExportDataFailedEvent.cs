using System;

namespace Sheaft.Application.Events
{
    public class AccountExportDataFailedEvent : Event
    {
        public const string QUEUE_NAME = "accountexportdatafailed";
        public const string MAILING_TEMPLATE_ID = "";

        public AccountExportDataFailedEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public Guid JobId { get; set; }
    }
}
