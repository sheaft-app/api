using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Producer
{
    public class ProducerDocumentsNotValidatedEvent : DomainEvent
    {
        [JsonConstructor]
        public ProducerDocumentsNotValidatedEvent(Guid producerId)
        {
            ProducerId = producerId;
        }

        public Guid ProducerId { get; }
    }
}
