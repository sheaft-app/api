using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderDeliveriesCommand : Command<bool>
    {
        public UpdateQuickOrderDeliveriesCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
