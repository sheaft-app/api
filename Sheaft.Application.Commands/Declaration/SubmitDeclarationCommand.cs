using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class SubmitDeclarationCommand : Command<bool>
    {
        [JsonConstructor]
        public SubmitDeclarationCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        public Guid LegalId { get; set; }
    }
}
