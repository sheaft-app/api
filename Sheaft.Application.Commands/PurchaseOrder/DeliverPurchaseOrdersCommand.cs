using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeliverPurchaseOrdersCommand : Command<bool>
    {
        public DeliverPurchaseOrdersCommand(RequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
