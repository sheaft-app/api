using System;

namespace Sheaft.Application.Commands
{
    public class SetDefaultQuickOrderCommand : Command<bool>
    {
        public SetDefaultQuickOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
