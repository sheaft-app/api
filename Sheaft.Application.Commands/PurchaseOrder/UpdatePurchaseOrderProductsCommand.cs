using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdatePurchaseOrderProductsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdatePurchaseOrderProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
