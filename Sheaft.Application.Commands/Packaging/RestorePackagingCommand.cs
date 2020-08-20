using System;

namespace Sheaft.Application.Commands
{
    public class RestorePackagingCommand : Command<bool>
    {
        public RestorePackagingCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
