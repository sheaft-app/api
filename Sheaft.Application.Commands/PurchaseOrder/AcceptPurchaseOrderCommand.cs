using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class AcceptPurchaseOrderCommand : Command<bool>
    {
        public AcceptPurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}