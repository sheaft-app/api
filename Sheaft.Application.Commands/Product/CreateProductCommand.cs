using System;

namespace Sheaft.Application.Commands
{
    public class CreateProductCommand : ProductCommand<Guid>
    {
        public CreateProductCommand(Interop.IRequestUser user) : base(user)
        {
        }

    }
}
