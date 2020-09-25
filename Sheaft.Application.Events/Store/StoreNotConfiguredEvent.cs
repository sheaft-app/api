using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class StoreNotConfiguredEvent : Event
    {
        public const string QUEUE_NAME = "event-store-not-configured";

        [JsonConstructor]
        public StoreNotConfiguredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
    }
}
