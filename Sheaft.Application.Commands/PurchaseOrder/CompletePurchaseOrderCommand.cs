using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CompletePurchaseOrderCommand : Command<bool>
    {
        public CompletePurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}