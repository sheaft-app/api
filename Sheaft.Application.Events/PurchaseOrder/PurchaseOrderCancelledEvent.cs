using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCancelledEvent : Event
    {
        [JsonConstructor]
        public PurchaseOrderCancelledEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
