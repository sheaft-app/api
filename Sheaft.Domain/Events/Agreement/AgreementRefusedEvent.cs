using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
