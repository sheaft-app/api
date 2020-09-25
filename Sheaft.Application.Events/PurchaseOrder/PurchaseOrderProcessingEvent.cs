using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorder-processing";

        [JsonConstructor]
        public PurchaseOrderProcessingEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
