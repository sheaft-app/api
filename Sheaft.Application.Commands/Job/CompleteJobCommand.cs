using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CompleteJobCommand : Command<bool>
    {
        public CompleteJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string FileUrl { get; set; }
    }
}
