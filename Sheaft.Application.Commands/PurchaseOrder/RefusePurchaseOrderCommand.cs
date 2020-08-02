using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RefusePurchaseOrderCommand : Command<bool>
    {
        public RefusePurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}