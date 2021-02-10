using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Producer
{
    public class ProducerDeclarationNotValidatedEvent : DomainEvent
    {
        [JsonConstructor]
        public ProducerDeclarationNotValidatedEvent(Guid producerId)
        {
            ProducerId = producerId;
        }

        public Guid ProducerId { get; }
    }
}
