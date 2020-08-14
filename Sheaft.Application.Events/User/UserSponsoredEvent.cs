using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class UserSponsoredEvent : Event
    {
        public const string QUEUE_NAME = "event-users-sponsored";
        public const string MAILING_TEMPLATE_ID = "";

        public UserSponsoredEvent(Interop.IRequestUser user) : base(user)
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
