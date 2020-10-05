using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDeclarationNotValidatedEvent : Event
    {
        [JsonConstructor]
        public ProducerDeclarationNotValidatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
