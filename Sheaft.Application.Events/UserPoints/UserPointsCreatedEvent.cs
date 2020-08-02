using MediatR;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Events
{
    public class UserPointsCreatedEvent : Event
    {
        public const string QUEUE_NAME = "userpointscreatedevent";

        public UserPointsCreatedEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid UserId { get; set; }
        public PointKind Kind { get; set; }
        public int Points { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}