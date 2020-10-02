using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class PayoutRefundFailedEvent : Event
    {
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public PayoutRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public string RefundIdentifier { get; set; }
    }
}
