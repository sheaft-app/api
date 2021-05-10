using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Agreement
{
    public class PreAuthorizationFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PreAuthorizationFailedEvent(Guid preAuthorizationId)
        {
            PreAuthorizationId = preAuthorizationId;
        }

        public Guid PreAuthorizationId { get; }
    }
}