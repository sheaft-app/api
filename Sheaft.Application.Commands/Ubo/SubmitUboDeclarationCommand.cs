using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SubmitUboDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public SubmitUboDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid LegalId { get; set; }
    }
}
