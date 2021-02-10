using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class UserPointsCreatedEvent : DomainEvent
    {
        [JsonConstructor]
        public UserPointsCreatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public PointKind Kind { get; set; }
        public int Points { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}