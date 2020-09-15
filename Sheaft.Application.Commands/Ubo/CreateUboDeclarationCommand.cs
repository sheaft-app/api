using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateUboDeclarationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateUboDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid LegalId { get; set; }

    }
}
