using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class GenerateUserSponsoringCodeCommand : Command<string>
    {
        [JsonConstructor]
        public GenerateUserSponsoringCodeCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
