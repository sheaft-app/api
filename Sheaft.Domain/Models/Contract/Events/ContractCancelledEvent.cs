using System;
using Newtonsoft.Json;

namespace Sheaft.Domain.Events.Contract
{
    public class ContractCancelledEvent : DomainEvent
    {
        [JsonConstructor]
        public ContractCancelledEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }

        public Guid AgreementId { get; }
    }
}
