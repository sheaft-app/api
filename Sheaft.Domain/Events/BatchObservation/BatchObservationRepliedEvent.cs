using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.BatchObservation
{
    public class BatchObservationRepliedEvent : DomainEvent
    {
        [JsonConstructor]
        public BatchObservationRepliedEvent(Guid batchObservationId, Guid replyId)
        {
            BatchObservationId = batchObservationId;
            ReplyId = replyId;
        }

        public Guid ReplyId { get; }

        public Guid BatchObservationId { get; }
    }
}
