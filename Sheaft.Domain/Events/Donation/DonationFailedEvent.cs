using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Donation
{
    public class DonationFailedEvent : DomainEvent
    {        
        [JsonConstructor]
        public DonationFailedEvent(Guid donationId)
        {
            DonationId = donationId;
        }

        public Guid DonationId { get; }
    }
}
