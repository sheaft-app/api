using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeletePackagingCommand : Command<bool>
    {
        public DeletePackagingCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
