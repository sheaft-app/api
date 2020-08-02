using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RetryJobCommand : Command<bool>
    {
        public RetryJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
