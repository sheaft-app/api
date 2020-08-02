using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ShipPurchaseOrdersCommand : Command<bool>
    {
        public ShipPurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
