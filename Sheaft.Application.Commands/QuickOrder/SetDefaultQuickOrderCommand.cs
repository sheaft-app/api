using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetDefaultQuickOrderCommand : Command<bool>
    {
        public SetDefaultQuickOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
