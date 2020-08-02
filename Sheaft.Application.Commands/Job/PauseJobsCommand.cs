using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class PauseJobsCommand : Command<bool>
    {
        public PauseJobsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
