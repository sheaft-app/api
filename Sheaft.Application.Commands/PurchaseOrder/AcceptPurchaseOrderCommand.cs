using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class AcceptPurchaseOrderCommand : Command<bool>
    {
        public AcceptPurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}