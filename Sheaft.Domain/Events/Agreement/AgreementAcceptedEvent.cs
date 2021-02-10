using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class AgreementAcceptedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementAcceptedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
