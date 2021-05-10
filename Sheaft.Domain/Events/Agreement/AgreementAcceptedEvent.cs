using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain.Events.Agreement
{
    public class AgreementAcceptedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementAcceptedEvent(Guid agreementId, ProfileKind requestedByKind)
        {
            AgreementId = agreementId;
            RequestedByKind = requestedByKind;
        }

        public Guid AgreementId { get; }
        public ProfileKind RequestedByKind { get; set; }
    }
}
