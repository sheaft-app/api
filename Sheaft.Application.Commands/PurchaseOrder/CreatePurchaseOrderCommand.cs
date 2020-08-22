using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{

    public class CreatePurchaseOrderCommand : Command<Guid>
    {
        public CreatePurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid ProducerId { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
        public Guid DeliveryModeId { get; set; }
        public DateTimeOffset ExpectedDeliveryDate { get; set; }
        public string Comment { get; set; }
        public bool SkipSendEmail { get; set; }
    }
}
