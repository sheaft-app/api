using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateProductCommand : ProductCommand<Guid>
    {
        public CreateProductCommand(RequestUser user) : base(user)
        {
        }

    }
}
