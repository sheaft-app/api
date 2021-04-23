using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain.Events.Agreement
{
    public class AgreementCancelledEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementCancelledEvent(Guid agreementId, ProfileKind requestedByKind)
        {
            AgreementId = agreementId;
            RequestedByKind = requestedByKind;
        }

        public Guid AgreementId { get; }
        public ProfileKind RequestedByKind { get; set; }
    }
}
