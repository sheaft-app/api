using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderProductsCommand : Command<bool>
    {
        public UpdateQuickOrderProductsCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
