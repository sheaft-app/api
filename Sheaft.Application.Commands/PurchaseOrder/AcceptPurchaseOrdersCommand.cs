using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class AcceptPurchaseOrdersCommand : Command<bool>
    {
        public AcceptPurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}