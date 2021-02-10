using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PickingOrder
{
    public class PickingOrderExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public PickingOrderExportProcessingEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
