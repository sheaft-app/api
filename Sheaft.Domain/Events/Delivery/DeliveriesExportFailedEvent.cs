using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class DeliveriesExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeliveriesExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
