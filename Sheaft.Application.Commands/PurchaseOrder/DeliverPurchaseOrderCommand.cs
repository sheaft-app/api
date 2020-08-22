using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeliverPurchaseOrderCommand : Command<bool>
    {
        public DeliverPurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
