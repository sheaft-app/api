using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteProductsCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
    }
}
