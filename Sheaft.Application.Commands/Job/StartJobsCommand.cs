using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class StartJobsCommand : Command<bool>
    {
        public StartJobsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
