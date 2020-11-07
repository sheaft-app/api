using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class StoreRegisteredEvent : Event
    {
        [JsonConstructor]
        public StoreRegisteredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
    }
}
