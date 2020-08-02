using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteProductsCommand : Command<bool>
    {
        public DeleteProductsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
