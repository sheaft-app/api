using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateProductCommand : ProductCommand<bool>
    {
        [JsonConstructor]
        public UpdateProductCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
