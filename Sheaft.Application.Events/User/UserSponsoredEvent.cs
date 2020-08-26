using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class UserSponsoredEvent : Event
    {
        public const string QUEUE_NAME = "event-users-sponsored";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public UserSponsoredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid SponsorId { get; set; }
        public Guid SponsoredId { get; set; }
    }

    public class SponsoringUsedEvent
    {
    }

    public class NewSponsoredEvent
    {
    }
}
