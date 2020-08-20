using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class RestoreTagCommand : Command<bool>
    {
        public RestoreTagCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
