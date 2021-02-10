using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Product
{
    public class ProductImportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public ProductImportFailedEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
