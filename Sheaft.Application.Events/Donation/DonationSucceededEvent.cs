using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DonationSucceededEvent : Event
    {
        public const string QUEUE_NAME = "event-donation-succeeded";
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DonationSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
