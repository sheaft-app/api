using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class PurchaseOrdersExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrdersExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
