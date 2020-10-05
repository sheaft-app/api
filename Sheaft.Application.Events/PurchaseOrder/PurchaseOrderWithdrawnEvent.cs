using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderWithdrawnEvent : Event
    {
        [JsonConstructor]
        public PurchaseOrderWithdrawnEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
