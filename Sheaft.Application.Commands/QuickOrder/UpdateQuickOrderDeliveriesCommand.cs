using System;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderDeliveriesCommand : Command<bool>
    {
        public UpdateQuickOrderDeliveriesCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
