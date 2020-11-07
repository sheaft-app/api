using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProducerDeclarationRequiredEvent : Event
    {
        [JsonConstructor]
        public ProducerDeclarationRequiredEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
