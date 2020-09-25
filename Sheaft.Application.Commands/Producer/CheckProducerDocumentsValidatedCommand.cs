using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class CheckProducerDocumentsValidatedCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckProducerDocumentsValidatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
