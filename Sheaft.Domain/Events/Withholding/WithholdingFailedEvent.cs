using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Withholding
{
    public class WithholdingFailedEvent : DomainEvent
    {
        [JsonConstructor]
        public WithholdingFailedEvent(Guid withholdingId)
        {
            WithholdingId = withholdingId;
        }

        public Guid WithholdingId { get; }
    }
}
