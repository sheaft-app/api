using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ResumeJobCommand : Command<bool>
    {
        public ResumeJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
