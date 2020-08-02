using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CompletePurchaseOrdersCommand : Command<bool>
    {
        public CompletePurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}