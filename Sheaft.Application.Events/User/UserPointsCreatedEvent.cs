using Sheaft.Core;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Events
{
    public class UserPointsCreatedEvent : Event
    {
        public const string QUEUE_NAME = "event-users-created-points";

        public UserPointsCreatedEvent(RequestUser user) : base(user)
        {
        }

        public Guid UserId { get; set; }
        public PointKind Kind { get; set; }
        public int Points { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}