using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class WithholdingFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public WithholdingFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid WithholdingId { get; set; }
    }
}
