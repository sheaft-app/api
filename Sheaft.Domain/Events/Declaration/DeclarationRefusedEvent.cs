using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Declaration
{
    public class DeclarationRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeclarationRefusedEvent(Guid declarationId)
        {
            DeclarationId = declarationId;
        }

        public Guid DeclarationId { get; }
    }
}
