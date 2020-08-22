using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateProductCommand : ProductCommand<bool>
    {
        public UpdateProductCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
