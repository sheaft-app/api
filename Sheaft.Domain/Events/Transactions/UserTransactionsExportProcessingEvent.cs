using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transactions
{
    public class UserTransactionsExportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public UserTransactionsExportProcessingEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
