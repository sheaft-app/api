using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class EnsureProducerDocumentsValidatedCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureProducerDocumentsValidatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
