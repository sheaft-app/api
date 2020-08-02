using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ShipPurchaseOrderCommand : Command<bool>
    {
        public ShipPurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
