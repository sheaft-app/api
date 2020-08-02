using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeliverPurchaseOrderCommand : Command<bool>
    {
        public DeliverPurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
