using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RefusePurchaseOrderCommand : Command<bool>
    {
        public RefusePurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}