using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Donation
{
    public class CreateDonationFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public CreateDonationFailedEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
