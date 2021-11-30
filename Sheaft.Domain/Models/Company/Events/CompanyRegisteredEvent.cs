using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Store
{
    public class StoreRegisteredEvent : DomainEvent
    {
        [JsonConstructor]
        public StoreRegisteredEvent(Guid storeId)
        {
            StoreId = storeId;
        }

        public Guid StoreId { get; }
    }
}
