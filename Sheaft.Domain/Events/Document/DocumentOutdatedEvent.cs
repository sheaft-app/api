using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class DocumentOutdatedEvent : DomainEvent
    {
        [JsonConstructor]
        public DocumentOutdatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
}
