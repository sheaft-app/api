using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Payin
{
    public class PayinSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public PayinSucceededEvent(Guid payinId)
        {
            PayinId = payinId;
        }

        public Guid PayinId { get; }
    }
}
