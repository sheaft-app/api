using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderRefusedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorders-refused";
        public const string MAILING_TEMPLATE_ID = "d-05a0d138172d4e9ab46b30c139c5f72e";

        public PurchaseOrderRefusedEvent(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
