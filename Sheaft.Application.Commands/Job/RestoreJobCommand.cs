using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreJobCommand : Command<bool>
    {
        public RestoreJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
