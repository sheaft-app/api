using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CancelJobCommand : Command<bool>
    {
        public CancelJobCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
