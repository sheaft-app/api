using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class FailJobCommand : Command<bool>
    {
        public FailJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
