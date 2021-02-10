using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class AgreementCreatedEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementCreatedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
