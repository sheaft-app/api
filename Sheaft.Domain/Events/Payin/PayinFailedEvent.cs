using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Payin
{
    public class PayinFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PayinFailedEvent(Guid payinId)
        {
            PayinId = payinId;
        }

        public Guid PayinId { get; }
    }
}
