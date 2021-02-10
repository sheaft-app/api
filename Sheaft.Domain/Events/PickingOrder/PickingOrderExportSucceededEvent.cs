using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PickingOrder
{
    public class PickingOrderExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public PickingOrderExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }
        
        public Guid JobId { get; }
    }
}
