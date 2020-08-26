using Sheaft.Models.Inputs;
using System;
using System.Collections.Generic;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdateQuickOrderProductsCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateQuickOrderProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
