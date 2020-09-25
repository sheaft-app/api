using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class CheckProducerDocumentsCreatedCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckProducerDocumentsCreatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
