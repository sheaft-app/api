using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RetryJobCommand : Command<bool>
    {
        public RetryJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
