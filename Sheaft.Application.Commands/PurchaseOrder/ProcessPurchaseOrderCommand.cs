using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ProcessPurchaseOrderCommand : Command<bool>
    {
        public ProcessPurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}