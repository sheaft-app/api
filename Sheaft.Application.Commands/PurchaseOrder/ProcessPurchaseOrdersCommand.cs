using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ProcessPurchaseOrdersCommand : Command<bool>
    {
        public ProcessPurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}