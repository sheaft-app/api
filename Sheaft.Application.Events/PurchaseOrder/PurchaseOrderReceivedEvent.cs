using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderReceivedEvent : Event
    {
        [JsonConstructor]
        public PurchaseOrderReceivedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
