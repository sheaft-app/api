using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class WithholdingFailedEvent : Event
    {
        [JsonConstructor]
        public WithholdingFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid WithholdingId { get; set; }
    }
}
