using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeliverPurchaseOrdersCommand : Command<bool>
    {
        public DeliverPurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
