using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckLegalsDeclarationRequiredCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckLegalsDeclarationRequiredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
