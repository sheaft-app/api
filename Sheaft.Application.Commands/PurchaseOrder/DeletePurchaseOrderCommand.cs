using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeletePurchaseOrderCommand : Command<bool>
    {
        public DeletePurchaseOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
