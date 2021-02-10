using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Payout
{
    public class PayoutFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PayoutFailedEvent(Guid payoutId)
        {
            PayoutId = payoutId;
        }

        public Guid PayoutId { get; }
    }
}
