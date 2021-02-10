using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PickingOrder
{
    public class PickingOrderExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PickingOrderExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
