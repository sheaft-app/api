using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UpdatePurchaseOrderProductsCommand : Command<bool>
    {
        public UpdatePurchaseOrderProductsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
