using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCancelledBySenderEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorder-cancelled-sender";
        public const string MAILING_TEMPLATE_ID = "d-6cd1a1b5d8d444a7a987ee8980d84ef3";

        [JsonConstructor]
        public PurchaseOrderCancelledBySenderEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
