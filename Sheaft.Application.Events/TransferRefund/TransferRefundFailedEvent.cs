using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class TransferRefundFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-transfer-refund-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public TransferRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RefundId { get; set; }
    }
}
