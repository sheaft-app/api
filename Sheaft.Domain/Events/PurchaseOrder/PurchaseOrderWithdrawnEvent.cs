using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderWithdrawnEvent : DomainEvent
    {
        [JsonConstructor]
        public PurchaseOrderWithdrawnEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
