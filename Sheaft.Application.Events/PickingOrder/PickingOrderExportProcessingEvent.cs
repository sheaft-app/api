using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-pickingorders-export-processing";

        public PickingOrderExportProcessingEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
