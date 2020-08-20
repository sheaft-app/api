using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RestorePurchaseOrderCommand : Command<bool>
    {
        public RestorePurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
