using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Agreement
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
