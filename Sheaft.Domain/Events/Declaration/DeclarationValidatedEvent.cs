using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class DeclarationValidatedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeclarationValidatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }
}
