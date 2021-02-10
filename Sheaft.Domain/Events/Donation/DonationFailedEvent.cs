using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class DonationFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public DonationFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
