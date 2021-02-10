using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PayinSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public PayinSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PayinId { get; set; }
    }
}
