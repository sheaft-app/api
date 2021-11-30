using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Product
{
    public class ProductImportSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public ProductImportSucceededEvent(Guid jobId)
        {
            JobId = jobId;
        }

        public Guid JobId { get; }
    }
}
