using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class PurchaseOrdersExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrdersExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }
        
        public Guid JobId { get; }
    }
}
