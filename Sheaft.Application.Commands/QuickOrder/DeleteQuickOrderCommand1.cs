using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteQuickOrderCommand : Command<bool>
    {
        public DeleteQuickOrderCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
