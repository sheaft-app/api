using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class TransactionsExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public TransactionsExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }
        
        public Guid JobId { get; }
    }
}
