using System;
using System.Collections.Generic;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateQuickOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductQuantityInput> Products { get; set; }
    }
}
