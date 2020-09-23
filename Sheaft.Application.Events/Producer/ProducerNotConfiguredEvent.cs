using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerNotConfiguredEvent : Event
    {
        public const string QUEUE_NAME = "event-producer-not-configured";

        [JsonConstructor]
        public ProducerNotConfiguredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
