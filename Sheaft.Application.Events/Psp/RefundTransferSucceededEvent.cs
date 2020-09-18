using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class RefundTransferSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-refund-transfer-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public RefundTransferSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
