using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class CreatePurchaseOrderTransferFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-create-purchaseorder-transfer-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public CreatePurchaseOrderTransferFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
