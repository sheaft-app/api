using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ResetJobCommand : Command<bool>
    {
        public ResetJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
