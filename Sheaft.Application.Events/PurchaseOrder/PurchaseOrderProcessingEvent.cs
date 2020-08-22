using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderProcessingEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorders-processing";

        public PurchaseOrderProcessingEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
