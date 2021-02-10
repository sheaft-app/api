using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Producer
{
    public class ProducerDeclarationRequiredEvent : DomainEvent
    {
        [JsonConstructor]
        public ProducerDeclarationRequiredEvent(Guid producerId)
        {
            ProducerId = producerId;
        }

        public Guid ProducerId { get; }
    }
}
