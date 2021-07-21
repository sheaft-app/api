using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class BillingExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public BillingExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }
        
        public Guid JobId { get; }
    }
}
