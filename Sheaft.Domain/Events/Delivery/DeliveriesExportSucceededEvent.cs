using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class DeliveriesExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveriesExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }
        
        public Guid JobId { get; }
    }
}
