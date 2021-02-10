using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Document
{
    public class DocumentRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public DocumentRefusedEvent(Guid documentId)
        {
            DocumentId = documentId;
        }

        public Guid DocumentId { get; }
    }
}
