using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class RestoreTagCommand : Command<bool>
    {
        public RestoreTagCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
