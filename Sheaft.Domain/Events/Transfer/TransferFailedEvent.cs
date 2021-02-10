using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Transfer
{
    public class TransferFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public TransferFailedEvent(Guid transferId)
        {
            TransferId = transferId;
        }

        public Guid TransferId { get; }
    }
}
