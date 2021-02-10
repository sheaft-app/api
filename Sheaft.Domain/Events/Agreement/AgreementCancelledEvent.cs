using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Models.Common;

namespace Sheaft.Application.Events
{
    public class AgreementCancelledEvent : DomainEvent
    {
        [JsonConstructor]
        public AgreementCancelledEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
