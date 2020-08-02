using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ResumeJobsCommand : Command<bool>
    {
        public ResumeJobsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
