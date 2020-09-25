using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderRefusedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorder-refused";
        public const string MAILING_TEMPLATE_ID = "d-05a0d138172d4e9ab46b30c139c5f72e";

        [JsonConstructor]
        public PurchaseOrderRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
