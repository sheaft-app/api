using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PickingOrderExportFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PickingOrderExportFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
