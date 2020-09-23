using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDocumentsNotValidatedEvent : Event
    {
        public const string QUEUE_NAME = "event-producer-documents-not-validated";

        [JsonConstructor]
        public ProducerDocumentsNotValidatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
