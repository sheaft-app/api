using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{

    public class CreatePurchaseOrderCommand : Command<Guid>
    {
        public CreatePurchaseOrderCommand(Interop.IRequestUser user) : base(user)
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
