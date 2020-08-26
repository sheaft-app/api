using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateProductCommand : ProductCommand<Guid>
    {
        [JsonConstructor]
        public CreateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
