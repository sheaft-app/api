using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Document
{
    public class DocumentOutdatedEvent : DomainEvent
    {
        [JsonConstructor]
        public DocumentOutdatedEvent(Guid documentId)
        {
            DocumentId = documentId;
        }

        public Guid DocumentId { get; }
    }
}
