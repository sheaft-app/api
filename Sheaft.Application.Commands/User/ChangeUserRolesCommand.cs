using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ChangeUserRolesCommand : Command<bool>
    {
        [JsonConstructor]
        public ChangeUserRolesCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
