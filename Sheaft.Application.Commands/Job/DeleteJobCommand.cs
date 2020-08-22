using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteJobCommand : Command<bool>
    {
        public DeleteJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
