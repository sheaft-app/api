using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RetryJobsCommand : Command<bool>
    {
        public RetryJobsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
