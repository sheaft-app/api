using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Batch
{
    public class BatchObservationAddedEvent : DomainEvent
    {
        [JsonConstructor]
        public BatchObservationAddedEvent(Guid batchId, Guid batchObservationId)
        {
            BatchId = batchId;
            BatchObservationId = batchObservationId;
        }

        public Guid BatchId { get; }

        public Guid BatchObservationId { get; }
    }
}