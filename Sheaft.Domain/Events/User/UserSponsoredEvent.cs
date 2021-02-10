using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.User
{
    public class UserSponsoredEvent : DomainEvent
    {
        [JsonConstructor]
        public UserSponsoredEvent(Guid sponsorId, Guid sponsoredId)
        {
            SponsorId = sponsorId;
            SponsoredId = sponsoredId;
            CreatedOn = DateTimeOffset.UtcNow;
        }

        public Guid SponsorId { get; }
        public Guid SponsoredId { get; }
        public DateTimeOffset CreatedOn { get; }
    }
}
