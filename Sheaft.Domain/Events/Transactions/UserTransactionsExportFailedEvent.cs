using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class UserTransactionsExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public UserTransactionsExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
