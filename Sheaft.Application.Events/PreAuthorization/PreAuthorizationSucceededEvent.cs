using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PreAuthorizationSucceededEvent : Event
    {
        [JsonConstructor]
        public PreAuthorizationSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PreAuthorizationId { get; set; }
    }
}
