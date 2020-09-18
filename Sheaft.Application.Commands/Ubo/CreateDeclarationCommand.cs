using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateDeclarationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }

    }
}
