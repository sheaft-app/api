using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class DeleteTagCommand : Command<bool>
    {
        public DeleteTagCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
