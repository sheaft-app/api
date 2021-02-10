using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class StoreRegisteredEvent : DomainEvent
    {
        [JsonConstructor]
        public StoreRegisteredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid StoreId { get; set; }
    }
}
