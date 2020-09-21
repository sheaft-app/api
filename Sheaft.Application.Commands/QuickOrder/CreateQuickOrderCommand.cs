using System;
using System.Collections.Generic;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

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
