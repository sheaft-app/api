using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class DeclarationRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public DeclarationRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }
}
