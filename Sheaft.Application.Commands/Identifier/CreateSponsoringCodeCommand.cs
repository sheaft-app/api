using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateSponsoringCodeCommand : Command<string>
    {
        [JsonConstructor]
        public CreateSponsoringCodeCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
