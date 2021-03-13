using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class UserTransactionsExportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public UserTransactionsExportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }
        
        public Guid JobId { get; }
    }
}
