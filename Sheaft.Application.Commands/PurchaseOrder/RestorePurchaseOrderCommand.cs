using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestorePurchaseOrderCommand : Command<bool>
    {
        public RestorePurchaseOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
