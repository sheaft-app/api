using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Events
{
    public class TransferRefundFailedEvent : Event
    {
        [JsonConstructor]
        public TransferRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RefundId { get; set; }
    }
}
