using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCompletedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorder-completed";
        public const string MAILING_TEMPLATE_ID = "d-3a8ee24d183241fcaa4ce259a334e2cf";

        [JsonConstructor]
        public PurchaseOrderCompletedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
