using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ResumeJobCommand : Command<bool>
    {
        public ResumeJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
