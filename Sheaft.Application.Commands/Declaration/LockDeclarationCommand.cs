using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class LockDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public LockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid DeclarationId { get; set; }
    }
}
