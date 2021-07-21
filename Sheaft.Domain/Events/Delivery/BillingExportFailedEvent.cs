using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class BillingExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public BillingExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
