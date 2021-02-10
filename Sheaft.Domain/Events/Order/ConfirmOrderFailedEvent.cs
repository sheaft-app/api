using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class ConfirmOrderFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public ConfirmOrderFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public string Message { get; set; }
    }
}
