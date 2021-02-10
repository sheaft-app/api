using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderAcceptedEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderAcceptedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
