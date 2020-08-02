using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ArchiveJobCommand : Command<bool>
    {
        public ArchiveJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
