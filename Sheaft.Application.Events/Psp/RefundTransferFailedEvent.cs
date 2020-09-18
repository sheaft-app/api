using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class RefundTransferFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-refund-transfer-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public RefundTransferFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
