using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class TransferSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-psp-transfer-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public TransferSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
