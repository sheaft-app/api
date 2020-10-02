using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class CreateTransferRefundFailedEvent : Event
    {
        [JsonConstructor]
        public CreateTransferRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
