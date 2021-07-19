using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Delivery
{
    public class AccountingExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public AccountingExportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
