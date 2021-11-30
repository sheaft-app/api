using System;
using Newtonsoft.Json;

namespace Sheaft.Domain.Events.Contract
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
