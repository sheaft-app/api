using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CompletePurchaseOrderCommand : Command<bool>
    {
        public CompletePurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}