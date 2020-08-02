using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class CreateQuickOrderCommand : Command<Guid>
    {
        public CreateQuickOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
