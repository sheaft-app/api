using System;
using Newtonsoft.Json;

namespace Sheaft.Domain.Events.Contract
{
    public class ContractAcceptedEvent : DomainEvent
    {
        [JsonConstructor]
        public ContractAcceptedEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }

        public Guid AgreementId { get; }
    }
}
