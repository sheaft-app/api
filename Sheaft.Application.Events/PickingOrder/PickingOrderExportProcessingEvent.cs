using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportProcessingEvent : Event
    {
        public const string QUEUE_NAME = "eventpickingorderexportprocessing";

        public PickingOrderExportProcessingEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
