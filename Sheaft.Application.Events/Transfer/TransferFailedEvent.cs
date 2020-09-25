using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class TransferFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-transfer-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public TransferFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }
}
