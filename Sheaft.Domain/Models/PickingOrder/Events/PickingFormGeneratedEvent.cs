using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Picking
{
    public class PickingFormGeneratedEvent : DomainEvent
    {
        [JsonConstructor]
        public PickingFormGeneratedEvent(Guid pickingId)
        {
            PickingId = pickingId;
        }

        public Guid PickingId { get; }
    }
}
