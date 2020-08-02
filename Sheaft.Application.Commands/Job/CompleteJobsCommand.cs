using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CompleteJobsCommand : Command<bool>
    {
        public CompleteJobsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
