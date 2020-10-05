using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class ConfirmOrderFailedEvent : Event
    {
        [JsonConstructor]
        public ConfirmOrderFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public string Message { get; set; }
    }
}
