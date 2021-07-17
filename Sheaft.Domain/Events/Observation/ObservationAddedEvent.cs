using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Observation
{
    public class ObservationAddedEvent : DomainEvent
    {
        [JsonConstructor]
        public ObservationAddedEvent(Guid batchId, Guid observationId)
        {
            BatchId = batchId;
            ObservationId = observationId;
        }

        public Guid BatchId { get; }

        public Guid ObservationId { get; }
    }
}