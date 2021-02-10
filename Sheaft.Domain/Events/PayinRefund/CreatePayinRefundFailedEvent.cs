using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class CreatePayinRefundFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public CreatePayinRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
