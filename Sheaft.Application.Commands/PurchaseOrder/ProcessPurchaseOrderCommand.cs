using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ProcessPurchaseOrderCommand : Command<bool>
    {
        public ProcessPurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}