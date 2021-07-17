using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Observation
{
    public class ObservationRepliedEvent : DomainEvent
    {
        [JsonConstructor]
        public ObservationRepliedEvent(Guid observationId, Guid replyId)
        {
            ObservationId = observationId;
            ReplyId = replyId;
        }

        public Guid ReplyId { get; }

        public Guid ObservationId { get; }
    }
}
