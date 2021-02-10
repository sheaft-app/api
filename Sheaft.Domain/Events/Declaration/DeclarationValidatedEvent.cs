using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Declaration
{
    public class DeclarationValidatedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeclarationValidatedEvent(Guid declarationId)
        {
            DeclarationId = declarationId;
        }

        public Guid DeclarationId { get; }
    }
}
