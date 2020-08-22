using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-pickingorders-export-failed";
        public const string MAILING_TEMPLATE_ID = "d-da95d609f77a43b586e39a404f9d5977";

        public PickingOrderExportFailedEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
