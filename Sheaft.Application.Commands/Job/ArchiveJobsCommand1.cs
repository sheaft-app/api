using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ArchiveJobsCommand : Command<bool>
    {
        public ArchiveJobsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
