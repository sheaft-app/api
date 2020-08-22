using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ArchiveJobCommand : Command<bool>
    {
        public ArchiveJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
