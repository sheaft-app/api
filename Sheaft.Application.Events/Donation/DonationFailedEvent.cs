using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DonationFailedEvent : Event
    {
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public DonationFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
