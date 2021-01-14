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

        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public Guid CardId { get; set; }
    }
}
