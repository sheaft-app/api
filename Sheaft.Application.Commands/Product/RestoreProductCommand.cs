using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RestoreProductCommand : Command<bool>
    {
        public RestoreProductCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
