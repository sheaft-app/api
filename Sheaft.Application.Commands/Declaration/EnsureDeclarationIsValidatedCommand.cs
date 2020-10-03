using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class EnsureDeclarationIsValidatedCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureDeclarationIsValidatedCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
