using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderReceivedEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderReceivedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
