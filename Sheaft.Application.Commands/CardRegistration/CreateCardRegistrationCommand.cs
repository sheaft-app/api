using System;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Application.Models;

namespace Sheaft.Application.Commands
{
    public class CreateCardRegistrationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateCardRegistrationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
