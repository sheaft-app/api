using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Observation
{
    public class ObservationAddedEvent : DomainEvent
    {
        [JsonConstructor]
        public ObservationAddedEvent(Guid observationId)
        {
            ObservationId = observationId;
        }

        public Guid ObservationId { get; }
    }
}