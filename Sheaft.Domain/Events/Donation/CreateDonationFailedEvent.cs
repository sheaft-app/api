using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class CreateDonationFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public CreateDonationFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
