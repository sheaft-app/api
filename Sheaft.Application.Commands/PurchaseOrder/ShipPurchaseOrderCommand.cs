using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ShipPurchaseOrderCommand : Command<bool>
    {
        public ShipPurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
