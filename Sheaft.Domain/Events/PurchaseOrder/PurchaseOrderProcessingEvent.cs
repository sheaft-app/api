using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderProcessingEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
