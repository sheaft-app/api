using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Events
{
    public class AgreementRefusedEvent : Event
    {
        [JsonConstructor]
        public AgreementRefusedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid AgreementId { get; set; }
    }
}
