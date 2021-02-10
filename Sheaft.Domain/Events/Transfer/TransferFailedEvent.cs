using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class TransferFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public TransferFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransferId { get; set; }
    }
}
