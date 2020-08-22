using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class PauseJobCommand : Command<bool>
    {
        public PauseJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
