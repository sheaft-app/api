using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class StoreNotConfiguredEvent : Event
    {
        [JsonConstructor]
        public StoreNotConfiguredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
    }
}
