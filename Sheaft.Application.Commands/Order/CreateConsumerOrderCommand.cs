using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class CreateConsumerOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public DonationKind Donation { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}
