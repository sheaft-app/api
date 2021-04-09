using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Agreement
{
    public class PreAuthorizationSucceededEvent : DomainEvent
    {
        [JsonConstructor]
        public PreAuthorizationSucceededEvent(Guid preAuthorizationId)
        {
            PreAuthorizationId = preAuthorizationId;
        }

        public Guid PreAuthorizationId { get; }
    }
}