using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteUsersCommand : Command<bool>
    {
        [JsonConstructor]
        public DeleteUsersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}
