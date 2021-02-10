using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class DeclarationIncompleteEvent : DomainEvent
    {
        [JsonConstructor]
        public DeclarationIncompleteEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DeclarationId { get; set; }
    }
}
