using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreatePreAuthorizationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePreAuthorizationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid OrderId { get; set; }
        public string CardIdentifier { get; set; }
    }
}
