using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PayoutFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PayoutFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayoutId { get; set; }
    }
}
