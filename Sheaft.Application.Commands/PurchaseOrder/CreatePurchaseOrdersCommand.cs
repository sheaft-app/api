using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrdersCommand : Command<IEnumerable<Guid>>
    {
        public CreatePurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public IEnumerable<ProducerExpectedDeliveryInput> ProducersExpectedDeliveries { get; set; }
    }
}
