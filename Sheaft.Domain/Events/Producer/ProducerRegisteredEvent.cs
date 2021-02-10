using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Producer
{
    public class ProducerRegisteredEvent : DomainEvent
    {
        [JsonConstructor]
        public ProducerRegisteredEvent(Guid producerId)
        {
            ProducerId = producerId;
        }

        public Guid ProducerId { get; }
    }
}
