using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeletePurchaseOrdersCommand : Command<bool>
    {
        public DeletePurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
