using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrdersCommand : Command<IEnumerable<Guid>>
    {
        [JsonConstructor]
        public CreatePurchaseOrdersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public decimal? Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}
