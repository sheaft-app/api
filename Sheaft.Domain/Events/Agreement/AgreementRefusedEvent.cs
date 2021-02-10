using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Agreement
{
    public class AgreementRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementRefusedEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }

        public Guid AgreementId { get; }
    }
}
