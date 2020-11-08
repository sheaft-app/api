using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SetProducerProductsWithNoVatCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProducerProductsWithNoVatCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid ProducerId { get; set; }
    }
}
