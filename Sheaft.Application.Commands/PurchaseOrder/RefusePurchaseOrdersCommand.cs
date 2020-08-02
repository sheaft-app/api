using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RefusePurchaseOrdersCommand : Command<bool>
    {
        public RefusePurchaseOrdersCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}