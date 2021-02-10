using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Product
{
    public class ProductImportProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public ProductImportProcessingEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
