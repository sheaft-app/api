using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDocumentsNotReviewedEvent : Event
    {
        [JsonConstructor]
        public ProducerDocumentsNotReviewedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
