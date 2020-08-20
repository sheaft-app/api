using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RestoreJobCommand : Command<bool>
    {
        public RestoreJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
