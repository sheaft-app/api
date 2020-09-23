using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDocumentsNotReviewedEvent : Event
    {
        public const string QUEUE_NAME = "event-producer-documents-not-reviewed";

        [JsonConstructor]
        public ProducerDocumentsNotReviewedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
