using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class PayinRefundFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public PayinRefundFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid RefundId { get; set; }
    }
}
