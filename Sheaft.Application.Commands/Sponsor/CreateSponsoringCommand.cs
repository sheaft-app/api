using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateSponsoringCommand : Command<bool>
    {
        [JsonConstructor]
        public CreateSponsoringCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Code { get; set; }
    }
}
