using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Agreement
{
    public class AgreementAcceptedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementAcceptedEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }
        
        public Guid AgreementId { get; }
    }
}
