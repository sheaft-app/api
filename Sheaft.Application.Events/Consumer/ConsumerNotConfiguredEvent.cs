using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ConsumerNotConfiguredEvent : Event
    {
        [JsonConstructor]
        public ConsumerNotConfiguredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ConsumerId { get; set; }
    }
}
