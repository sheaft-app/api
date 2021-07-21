using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class BillingExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public BillingExportProcessingEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
