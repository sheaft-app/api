using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportSucceededEvent : Event
    {
        public const string QUEUE_NAME = "eventpickingorderexportsucceeded";
        public const string MAILING_TEMPLATE_ID = "d-45f0c494fa0f48a9a7024b667cea16a0";

        public PickingOrderExportSucceededEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
