using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UnLockDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public UnLockDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid DeclarationId { get; set; }
    }
}
