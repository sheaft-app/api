using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class CreatePayinRefundFailedEvent : Event
    {
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public CreatePayinRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
