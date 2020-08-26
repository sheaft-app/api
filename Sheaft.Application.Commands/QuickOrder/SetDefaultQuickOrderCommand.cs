using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetDefaultQuickOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public SetDefaultQuickOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
