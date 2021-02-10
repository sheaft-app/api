using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class UserSponsoredEvent : DomainEvent
    {
        [JsonConstructor]
        public UserSponsoredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid SponsorId { get; set; }
        public Guid SponsoredId { get; set; }
    }
}
