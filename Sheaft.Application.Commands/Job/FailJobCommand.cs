using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class FailJobCommand : Command<bool>
    {
        public FailJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
