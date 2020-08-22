using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateQuickOrderCommand : Command<Guid>
    {
        public CreateQuickOrderCommand(RequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
