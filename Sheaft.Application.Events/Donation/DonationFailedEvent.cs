using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DonationFailedEvent : Event
    {
        [JsonConstructor]
        public DonationFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
