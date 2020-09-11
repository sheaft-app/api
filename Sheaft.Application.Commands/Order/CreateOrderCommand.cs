using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}
