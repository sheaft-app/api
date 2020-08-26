using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateContactCommand : Command<bool>
    {
        [JsonConstructor]
        public CreateContactCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
