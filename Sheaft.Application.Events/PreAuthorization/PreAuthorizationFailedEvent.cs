using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PreAuthorizationFailedEvent : Event
    {
        [JsonConstructor]
        public PreAuthorizationFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PreAuthorizationId { get; set; }
    }
}
