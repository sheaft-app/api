using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain.Events.User
{
    public class UserPointsCreatedEvent : DomainEvent
    {
        [JsonConstructor]
        public UserPointsCreatedEvent(Guid userId, PointKind kind, int points, DateTimeOffset createdOn)
        {
            UserId = userId;
            Kind = kind;
            Points = points;
            CreatedOn = createdOn;
        }

        public Guid UserId { get; }
        public PointKind Kind { get; }
        public int Points { get; }
        public DateTimeOffset CreatedOn { get; }
    }
}