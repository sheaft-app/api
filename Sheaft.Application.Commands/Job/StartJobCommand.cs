using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class StartJobCommand : Command<bool>
    {
        public StartJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
