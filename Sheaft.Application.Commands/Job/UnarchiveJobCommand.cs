using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UnarchiveJobCommand : Command<bool>
    {
        public UnarchiveJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
