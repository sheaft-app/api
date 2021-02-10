using System;
using Newtonsoft.Json;
using Sheaft.Domain.Common;

namespace Sheaft.Domain.Events.Agreement
{
    public class AgreementCancelledEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementCancelledEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }

        public Guid AgreementId { get; }
    }
}
