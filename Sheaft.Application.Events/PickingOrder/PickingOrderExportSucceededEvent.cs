﻿using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportSucceededEvent : Event
    {
        [JsonConstructor]
        public PickingOrderExportSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
