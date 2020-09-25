using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderAcceptedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorder-accepted";
        public const string MAILING_TEMPLATE_ID = "d-63757e1bde1c4942b28e50053a430167";

        [JsonConstructor]
        public PurchaseOrderAcceptedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
