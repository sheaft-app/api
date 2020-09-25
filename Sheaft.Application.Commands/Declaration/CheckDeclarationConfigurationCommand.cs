using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckDeclarationConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckDeclarationConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
