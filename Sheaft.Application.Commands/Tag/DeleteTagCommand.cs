using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteTagCommand : Command<bool>
    {
        public DeleteTagCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
