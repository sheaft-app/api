using System;
using Newtonsoft.Json;

namespace Sheaft.Domain.Events.Contract
{
    public class AgreementCreatedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementCreatedEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }

        public Guid AgreementId { get; }
    }
}
