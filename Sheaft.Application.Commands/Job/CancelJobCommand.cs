using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CancelJobCommand : Command<bool>
    {
        public CancelJobCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
