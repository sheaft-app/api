using System;

namespace Sheaft.Application.Commands
{
    public class UpdateProductCommand : ProductCommand<bool>
    {
        public UpdateProductCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
