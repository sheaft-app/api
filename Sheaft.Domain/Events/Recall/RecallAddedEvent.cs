using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Recall
{
    public class RecallSentEvent : DomainEvent
    {
        [JsonConstructor]
        public RecallSentEvent(Guid recallId, Guid clientId)
        {
            ClientId = clientId;
            RecallId = recallId;
        }

        public Guid RecallId { get; }
        public Guid ClientId { get; }
    }
}