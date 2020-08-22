using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCompletedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorders-completed";
        public const string MAILING_TEMPLATE_ID = "d-3a8ee24d183241fcaa4ce259a334e2cf";

        public PurchaseOrderCompletedEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
