using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ResumeJobsCommand : Command<bool>
    {
        public ResumeJobsCommand(RequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
