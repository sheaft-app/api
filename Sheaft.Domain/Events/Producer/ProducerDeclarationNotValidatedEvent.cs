using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class ProducerDeclarationNotValidatedEvent : DomainEvent
    {
        [JsonConstructor]
        public ProducerDeclarationNotValidatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
