using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class DocumentRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public DocumentRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
    }
}
