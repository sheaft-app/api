using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Declaration
{
    public class DeclarationIncompleteEvent : DomainEvent
    {
        [JsonConstructor]
        public DeclarationIncompleteEvent(Guid declarationId)
        {
            DeclarationId = declarationId;
        }

        public Guid DeclarationId { get; }
    }
}
