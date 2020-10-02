﻿using Sheaft.Core;
using System;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Events
{
    public class UserPointsCreatedEvent : Event
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