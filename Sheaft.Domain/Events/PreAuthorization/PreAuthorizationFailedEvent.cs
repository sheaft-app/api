using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.PreAuthorization
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