using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class EnsureDeclarationConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureDeclarationConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }

    }
}
