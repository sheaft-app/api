using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CancelPurchaseOrderCommand : Command<bool>
    {
        public CancelPurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}