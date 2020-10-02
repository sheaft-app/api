﻿using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Events
{
    public class AgreementCreatedEvent : Event
    {
        [JsonConstructor]
        public AgreementCreatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
