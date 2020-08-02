using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteProductCommand : Command<bool>
    {
        public DeleteProductCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
