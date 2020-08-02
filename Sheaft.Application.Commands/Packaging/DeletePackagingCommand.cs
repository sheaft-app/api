using System;

namespace Sheaft.Application.Commands
{
    public class DeletePackagingCommand : Command<bool>
    {
        public DeletePackagingCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
