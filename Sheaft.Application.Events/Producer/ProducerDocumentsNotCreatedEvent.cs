using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDocumentsNotCreatedEvent : Event
    {
        [JsonConstructor]
        public ProducerDocumentsNotCreatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
