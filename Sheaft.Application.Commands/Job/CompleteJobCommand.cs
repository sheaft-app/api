using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CompleteJobCommand : Command<bool>
    {
        public CompleteJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string FileUrl { get; set; }
    }
}
