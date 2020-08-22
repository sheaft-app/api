using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeletePurchaseOrderCommand : Command<bool>
    {
        public DeletePurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
