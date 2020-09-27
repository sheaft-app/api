using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class CreateDonationFailedEvent : Event
    {
        public const string QUEUE_NAME = "event-create-donation-failed";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public CreateDonationFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
