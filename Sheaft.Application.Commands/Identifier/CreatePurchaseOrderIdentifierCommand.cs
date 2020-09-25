using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrderIdentifierCommand : Command<string>
    {
        [JsonConstructor]
        public CreatePurchaseOrderIdentifierCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
