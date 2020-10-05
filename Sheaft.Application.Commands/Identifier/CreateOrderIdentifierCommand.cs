using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateOrderIdentifierCommand : Command<string>
    {
        [JsonConstructor]
        public CreateOrderIdentifierCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
