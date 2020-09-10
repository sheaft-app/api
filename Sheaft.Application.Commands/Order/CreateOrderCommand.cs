using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public decimal Donation { get; set; }
    }
}
