using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PayinFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PayinFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
}
