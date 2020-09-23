using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDocumentsNotCreatedEvent : Event
    {
        public const string QUEUE_NAME = "event-producer-documents-not-created";

        [JsonConstructor]
        public ProducerDocumentsNotCreatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
