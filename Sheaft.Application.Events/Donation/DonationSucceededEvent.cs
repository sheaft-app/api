using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class DonationSucceededEvent : Event
    {
        [JsonConstructor]
        public DonationSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
